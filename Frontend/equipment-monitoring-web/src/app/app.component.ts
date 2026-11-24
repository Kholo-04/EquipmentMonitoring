import { Component, inject } from '@angular/core';
import { DatePipe, DecimalPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {
  HttpClient,
  HttpErrorResponse
} from '@angular/common/http';


interface PredictionRequest {
  type: string;
  airTemperature: number;
  processTemperature: number;
  rotationalSpeed: number;
  torque: number;
  toolWear: number;
}

interface PredictionResponse {
  failurePrediction: boolean;
  prediction: string;
  failureProbability: number;
  noFailureProbability: number;
  maintenancePriority: string;
  model: string;
}

interface PredictionHistoryItem {

  predictionId: number;

  type: string;

  failurePrediction: boolean;

  failureProbability: number;

  maintenancePriority: string;

  modelName: string;

  createdDate: string;
}

interface EquipmentItem {

  equipmentId: number;

  equipmentName: string;

  equipmentType: string;

  status: string;

}

interface UserItem {

  userId: number;

  username: string;

  role: string;

  emailAddress: string;
}

interface DashboardMetrics {

  totalUsers: number;

  totalEquipment: number;

  totalPredictions: number;

  highPriorityPredictions: number;

  activeEquipment: number;

  inactiveEquipment: number;

  lowPriorityPredictions: number;

}

interface NotificationItem {

  notificationId: number;

  recipient: string;

  subject: string;

  status: string;

  dateSent: string;

}

interface AuditItem {

  auditLogId: number;

  username: string;

  action: string;

  module: string;

  description: string;

  dateCreated: string;
}


@Component({
  selector: 'app-root',
  standalone: true,

  imports: [
    FormsModule,
    DecimalPipe,
    DatePipe
  ],

  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {

  // -------------------------------------------------------
  // Services and configuration
  // -------------------------------------------------------

  private readonly http = inject(HttpClient);

  private readonly apiUrl =
    'https://localhost:7065/api/Predictions';

  private readonly historyStorageKey =
    'equipmentPredictionHistory';


  // -------------------------------------------------------
  // Authentication
  // -------------------------------------------------------

  isAuthenticated = false;

  username = '';

  password = '';

  newUsername = '';

  newPassword = '';

  newRole = 'Technician';

  loginError = '';

  currentUser = '';

  currentRole = '';

  userSuccessMessage = '';

  newEquipmentName = '';

  newEquipmentType = 'L';

  newEquipmentStatus = 'Active';

  editingUserId: number | null = null;

  editRole = '';

  editEmailAddress = '';

  editingEquipmentId: number | null = null;

  editEquipmentName = '';

  editEquipmentType = '';

  editEquipmentStatus = '';

  newEmailAddress = '';


  // -------------------------------------------------------
  // Navigation
  // -------------------------------------------------------

  currentView:
    | 'dashboard'
    | 'equipment'
    | 'users'
    | 'prediction'
    | 'history'
    | 'notifications'
    | 'audit' = 'dashboard';


  // -------------------------------------------------------
  // Prediction input
  // -------------------------------------------------------

  predictionRequest: PredictionRequest = {
    type: 'L',
    airTemperature: 300.5,
    processTemperature: 309.8,
    rotationalSpeed: 1345,
    torque: 62.7,
    toolWear: 153
  };


  // -------------------------------------------------------
  // Prediction state
  // -------------------------------------------------------

  predictionResult: PredictionResponse | null = null;

  predictionHistory: PredictionHistoryItem[] = [];
  equipmentList: EquipmentItem[] = [];
  userList: UserItem[] = [];
  notificationList: NotificationItem[] = [];
  auditList: AuditItem[] = [];

  dashboardMetrics: DashboardMetrics = {

    totalUsers: 0,

    totalEquipment: 0,

    totalPredictions: 0,

    highPriorityPredictions: 0,

    activeEquipment: 0,

    inactiveEquipment: 0,

    lowPriorityPredictions: 0

  };

  isPredicting = false;

  predictionError = '';


  // -------------------------------------------------------
  // Constructor
  // -------------------------------------------------------

  constructor() {

    this.loadPredictionHistory();
    this.loadEquipment();
    this.loadUsers();
    this.loadDashboard();
    this.restoreLoginSession();
    this.loadNotifications();
    this.loadAuditLogs();
  }

  // -------------------------------------------------------
  // Login
  // -------------------------------------------------------

  login(): void {

    this.loginError = '';

    if (
      !this.username.trim() ||
      !this.password.trim()
    ) {

      this.loginError =
        'Username and password are required.';

      return;
    }

    this.http
      .post<any>(
        'https://localhost:7065/api/Auth/login',
        {
          username: this.username,
          password: this.password
        }
      )
      .subscribe({

        next: (response) => {

          this.isAuthenticated = true;

          this.currentUser =
            response.username;

          this.currentRole =
            response.role;

          this.password = '';

          sessionStorage.setItem(
            'prototypeAuthenticated',
            'true'
          );

          sessionStorage.setItem(
            'prototypeUser',
            this.currentUser
          );

          sessionStorage.setItem(
            'prototypeRole',
            this.currentRole
          );
        },

        error: () => {

          this.loginError =
            'Invalid username or password.';
        }
      });
  }


  // -------------------------------------------------------
  // Restore login session
  // -------------------------------------------------------

  private restoreLoginSession(): void {

    const authenticated =
      sessionStorage.getItem(
        'prototypeAuthenticated'
      );

    if (authenticated !== 'true') {
      return;
    }

    this.isAuthenticated = true;

    this.currentUser =
      sessionStorage.getItem(
        'prototypeUser'
      ) ?? 'Prototype Manager';

    this.currentRole =
      sessionStorage.getItem(
        'prototypeRole'
      ) ?? 'Maintenance Manager';
  }


  // -------------------------------------------------------
  // Logout
  // -------------------------------------------------------

  logout(): void {

    this.isAuthenticated = false;

    this.username = '';
    this.password = '';
    this.loginError = '';

    this.currentUser = '';
    this.currentRole = '';

    this.currentView = 'dashboard';

    this.predictionResult = null;
    this.predictionError = '';

    sessionStorage.removeItem(
      'prototypeAuthenticated'
    );

    sessionStorage.removeItem(
      'prototypeUser'
    );

    sessionStorage.removeItem(
      'prototypeRole'
    );
  }


  // -------------------------------------------------------
  // Generate machine-failure prediction
  // -------------------------------------------------------

  generatePrediction(): void {

    this.predictionError = '';
    this.predictionResult = null;

    // Validate equipment type
    if (
      !['L', 'M', 'H']
        .includes(this.predictionRequest.type)
    ) {
      this.predictionError =
        'Equipment type must be L, M or H.';

      return;
    }

    // Validate rotational speed
    if (
      this.predictionRequest.rotationalSpeed <= 0
    ) {
      this.predictionError =
        'Rotational speed must be greater than zero.';

      return;
    }

    // Validate tool wear
    if (
      this.predictionRequest.toolWear < 0
    ) {
      this.predictionError =
        'Tool wear cannot be negative.';

      return;
    }

    this.isPredicting = true;

    this.http
      .post<PredictionResponse>(
        this.apiUrl,
        this.predictionRequest
      )
      .subscribe({

        next: (result) => {

          this.predictionResult = result;

          this.savePredictionToHistory(
            result
          );

          this.isPredicting = false;
        },

        error: (
          error: HttpErrorResponse
        ) => {

          console.error(
            'Prediction request failed:',
            error
          );

          if (error.status === 0) {
            this.predictionError =
              'The prediction service could not be reached. ' +
              'Confirm that the ASP.NET Core API and ' +
              'machine-learning service are running.';
          }

          else if (error.status === 400) {
            this.predictionError =
              error.error?.message ??
              'The prediction request contains invalid values.';
          }

          else if (error.status === 503) {
            this.predictionError =
              'The machine-learning service is currently unavailable.';
          }

          else if (error.status === 504) {
            this.predictionError =
              'The machine-learning service did not respond in time.';
          }

          else {
            this.predictionError =
              'Prediction could not be completed. Please try again.';
          }

          this.isPredicting = false;
        }

      });
  }


  // -------------------------------------------------------
  // Save prediction to history
  // -------------------------------------------------------

  private savePredictionToHistory(
    result: PredictionResponse
  ): void {

    const request = {

      type:
        this.predictionRequest.type,

      failurePrediction:
        result.failurePrediction,

      failureProbability:
        result.failureProbability,

      maintenancePriority:
        result.maintenancePriority,

      modelName:
        result.model
    };

    this.http
      .post(
        'https://localhost:7065/api/PredictionHistory/save',
        request
      )
      .subscribe({

        next: () => {

          console.log(
            'Prediction saved to SQL Server'
          );

          this.loadPredictionHistory();
        },

        error: (error) => {

          console.error(
            'Prediction save failed',
            error
          );
        }
      });
  }


  // -------------------------------------------------------
  // Load prediction history
  // -------------------------------------------------------

  private loadPredictionHistory(): void {

    this.http
      .get<PredictionHistoryItem[]>(
        'https://localhost:7065/api/PredictionHistory/history'
      )
      .subscribe({

        next: (history) => {

          this.predictionHistory = history;
        },

        error: (error) => {

          console.error(
            'Prediction history load failed',
            error
          );

          this.predictionHistory = [];
        }
      });
  }


  // -------------------------------------------------------
  // Clear prediction history
  // -------------------------------------------------------
  isAdministrator(): boolean {

    return this.currentRole === 'Administrator';

  }

  clearPredictionHistory(): void {

    this.predictionHistory = [];

    localStorage.removeItem(
      this.historyStorageKey
    );
  }

  private loadEquipment(): void {

    this.http
      .get<EquipmentItem[]>(
        'https://localhost:7065/api/Equipment'
      )
      .subscribe({

        next: (equipment) => {

          this.equipmentList =
            equipment;
        },

        error: (error) => {

          console.error(
            'Equipment load failed',
            error
          );
        }
      });
  }

  private loadUsers(): void {

    this.http
      .get<UserItem[]>(
        'https://localhost:7065/api/Users'
      )
      .subscribe({

        next: (users) => {

          this.userList = users;
        },

        error: (error) => {

          console.error(
            'User load failed',
            error
          );
        }
      });
  }

  createUser(): void {

    if (
      !this.newUsername.trim() ||
      !this.newPassword.trim() ||
      !this.newRole.trim()
    ) {
      return;
    }

    this.http
      .post(
        'https://localhost:7065/api/Users',
        {
          username: this.newUsername,
          password: this.newPassword,
          role: this.newRole,
          emailAddress: this.newEmailAddress
        }
      )
      .subscribe({

        next: () => {

          this.userSuccessMessage =
            '✅ User created successfully';

          this.newUsername = '';

          this.newPassword = '';

          this.newRole = 'Technician';

          this.newEmailAddress = '';

          this.loadUsers();

          setTimeout(() => {

            this.userSuccessMessage = '';

          }, 3000);
        },

        error: (error) => {

          console.error(
            'Create user failed',
            error
          );
        }
      });
  }

  deleteUser(
    userId: number
  ): void {

    if (
      !confirm(
        'Are you sure you want to delete this user?'
      )
    ) {
      return;
    }

    this.http
      .delete(
        `https://localhost:7065/api/Users/${userId}`
      )
      .subscribe({

        next: () => {

          this.loadUsers();
        },

        error: (error) => {

          console.error(
            'Delete user failed',
            error
          );
        }
      });
  }

  createEquipment(): void {

    if (
      !this.newEquipmentName.trim()
    ) {
      return;
    }

    this.http
      .post(
        'https://localhost:7065/api/Equipment',
        {
          equipmentName:
            this.newEquipmentName,

          equipmentType:
            this.newEquipmentType,

          status:
            this.newEquipmentStatus
        }
      )
      .subscribe({

        next: () => {

          this.newEquipmentName = '';

          this.newEquipmentType = 'L';

          this.newEquipmentStatus = 'Active';

          this.loadEquipment();
        },

        error: (error) => {

          console.error(
            'Create equipment failed',
            error
          );
        }
      });
  }

  deleteEquipment(
    equipmentId: number
  ): void {

    if (
      !confirm(
        'Delete this equipment?'
      )
    ) {
      return;
    }

    this.http
      .delete(
        `https://localhost:7065/api/Equipment/${equipmentId}`
      )
      .subscribe({

        next: () => {

          this.loadEquipment();
        },

        error: (error) => {

          console.error(
            'Delete equipment failed',
            error
          );
        }
      });
  }

  private loadDashboard(): void {

    this.http
      .get<DashboardMetrics>(
        'https://localhost:7065/api/Dashboard'
      )
      .subscribe({

        next: (metrics) => {

          this.dashboardMetrics =
            metrics;
        },

        error: (error) => {

          console.error(
            'Dashboard load failed',
            error
          );
        }
      });
  }

  startEditUser(
    user: UserItem
  ): void {

    this.editingUserId =
      user.userId;

    this.editRole =
      user.role;

    this.editEmailAddress =
      user.emailAddress ?? '';
  }
  saveUser(): void {

    if (
      this.editingUserId === null
    ) {
      return;
    }

    this.http
      .put(
        `https://localhost:7065/api/Users/${this.editingUserId}`,
        {
          role:
            this.editRole,

          emailAddress:
            this.editEmailAddress
        }
      )
      .subscribe({

        next: () => {

          this.editingUserId =
            null;

          this.loadUsers();
        },

        error: (error) => {

          console.error(
            'Update user failed',
            error
          );
        }
      });
  }

  startEditEquipment(
    equipment: EquipmentItem
  ): void {

    this.editingEquipmentId =
      equipment.equipmentId;

    this.editEquipmentName =
      equipment.equipmentName;

    this.editEquipmentType =
      equipment.equipmentType;

    this.editEquipmentStatus =
      equipment.status;
  }

  saveEquipment(): void {

    if (
      this.editingEquipmentId === null
    ) {
      return;
    }

    this.http
      .put(
        `https://localhost:7065/api/Equipment/${this.editingEquipmentId}`,
        {
          equipmentName:
            this.editEquipmentName,

          equipmentType:
            this.editEquipmentType,

          status:
            this.editEquipmentStatus
        }
      )
      .subscribe({

        next: () => {

          this.editingEquipmentId =
            null;

          this.loadEquipment();
        },

        error: (error) => {

          console.error(
            'Update equipment failed',
            error
          );
        }
      });
  }
  private loadNotifications(): void {

    this.http
      .get<NotificationItem[]>(
        'https://localhost:7065/api/Notifications'
      )
      .subscribe({

        next: (notifications) => {

          this.notificationList =
            notifications;
        },

        error: (error) => {

          console.error(
            'Notification load failed',
            error
          );
        }
      });
  }

  private loadAuditLogs(): void {

    this.http
      .get<AuditItem[]>(
        'https://localhost:7065/api/Audit'
      )
      .subscribe({

        next: (auditLogs) => {

          this.auditList =
            auditLogs;
        },

        error: (error) => {

          console.error(
            'Audit log load failed',
            error
          );
        }
      });
  }


}
