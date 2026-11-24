import joblib
import pandas as pd

MODEL_FILE = "decision_tree_predictive_maintenance.pkl"

# Load the trained preprocessing + Decision Tree pipeline
model = joblib.load(MODEL_FILE)

# Test equipment record using EXACTLY the same feature names
# that were used when the model was trained.
sample_input = pd.DataFrame([
    {
        "Type": "L",
        "Air temperature": 300.5,
        "Process temperature": 309.8,
        "Rotational speed": 1345,
        "Torque": 62.7,
        "Tool wear": 153
    }
])

# Generate prediction
prediction = model.predict(sample_input)[0]
probabilities = model.predict_proba(sample_input)[0]

no_failure_probability = float(probabilities[0])
failure_probability = float(probabilities[1])

print("Local Decision Tree Prediction")
print("==============================")
print()

print("Equipment Input")
print("---------------")
print(sample_input.to_string(index=False))
print()

print("Prediction Result")
print("-----------------")
print("Predicted class:", prediction)
print(
    "Prediction:",
    "Failure" if prediction == 1 else "No Failure"
)
print(
    "No Failure probability:",
    round(no_failure_probability, 4)
)
print(
    "Failure probability:",
    round(failure_probability, 4)
)