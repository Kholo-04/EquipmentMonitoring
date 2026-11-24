from pathlib import Path

import joblib
import pandas as pd
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel, Field


# ---------------------------------------------------------
# Application setup
# ---------------------------------------------------------

app = FastAPI(
    title="Equipment Monitoring ML Service",
    description="Decision Tree machine-failure prediction service",
    version="1.0.0"
)


# ---------------------------------------------------------
# Load trained model
# ---------------------------------------------------------

BASE_DIR = Path(__file__).resolve().parent
MODEL_PATH = BASE_DIR / "decision_tree_predictive_maintenance.pkl"

try:
    model = joblib.load(MODEL_PATH)
    print("Decision Tree model loaded successfully.")
except Exception as ex:
    print("Failed to load Decision Tree model.")
    print(ex)
    raise


# ---------------------------------------------------------
# Request model
# ---------------------------------------------------------

class PredictionRequest(BaseModel):
    type: str = Field(..., min_length=1)
    airTemperature: float
    processTemperature: float
    rotationalSpeed: float
    torque: float
    toolWear: float


# ---------------------------------------------------------
# Health endpoint
# ---------------------------------------------------------

@app.get("/")
def health_check():
    return {
        "status": "running",
        "service": "Equipment Monitoring ML Service",
        "model": "Decision Tree"
    }


# ---------------------------------------------------------
# Prediction endpoint
# ---------------------------------------------------------

@app.post("/predict")
def predict(request: PredictionRequest):

    try:

        # Basic validation
        equipment_type = request.type.upper()

        if equipment_type not in ["L", "M", "H"]:
            raise HTTPException(
                status_code=400,
                detail="Type must be L, M or H."
            )

        if request.rotationalSpeed <= 0:
            raise HTTPException(
                status_code=400,
                detail="Rotational speed must be greater than zero."
            )

        if request.toolWear < 0:
            raise HTTPException(
                status_code=400,
                detail="Tool wear cannot be negative."
            )

        # IMPORTANT:
        # These column names must exactly match the
        # columns used during model training.

        input_data = pd.DataFrame([
            {
                "Type": equipment_type,
                "Air temperature": request.airTemperature,
                "Process temperature": request.processTemperature,
                "Rotational speed": request.rotationalSpeed,
                "Torque": request.torque,
                "Tool wear": request.toolWear
            }
        ])

        prediction = int(model.predict(input_data)[0])

        probabilities = model.predict_proba(input_data)[0]

        no_failure_probability = float(probabilities[0])
        failure_probability = float(probabilities[1])

        prediction_label = (
            "Failure"
            if prediction == 1
            else "No Failure"
        )

        # Prototype maintenance priority
        if failure_probability >= 0.70:
            maintenance_priority = "High"
        elif failure_probability >= 0.40:
            maintenance_priority = "Medium"
        else:
            maintenance_priority = "Low"

        return {
            "failurePrediction": prediction == 1,
            "prediction": prediction_label,
            "failureProbability": round(
                failure_probability,
                4
            ),
            "noFailureProbability": round(
                no_failure_probability,
                4
            ),
            "maintenancePriority": maintenance_priority,
            "model": "Decision Tree"
        }

    except HTTPException:
        raise

    except Exception as ex:
        raise HTTPException(
            status_code=500,
            detail=f"Prediction failed: {str(ex)}"
        )