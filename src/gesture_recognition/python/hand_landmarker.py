import mediapipe as mp
from mediapipe.tasks import python
from mediapipe.tasks.python import vision
import mediapipe.python.solutions.hands as hands
import math
from typing import Union, Tuple
import numpy as np

def _normalized_to_pixel_coordinates(
    normalized_x: float, normalized_y: float, image_width: int,
    image_height: int) -> Union[None, Tuple[int, int]]:
    """Converts normalized value pair to pixel coordinates."""

    # Checks if the float value is between 0 and 1.
    def is_valid_normalized_value(value: float) -> bool:
        return (value > 0 or math.isclose(0, value)) and (value < 1 or
                                                      math.isclose(1, value))

    if not (is_valid_normalized_value(normalized_x) and
          is_valid_normalized_value(normalized_y)):
        return None
    x_px = min(math.floor(normalized_x * image_width), image_width - 1)
    y_px = min(math.floor(normalized_y * image_height), image_height - 1)
    return x_px, y_px

def detect(image_np: np.ndarray):
    # image_np: a numpy array in SRGB format
    base_options = python.BaseOptions(model_asset_path='hand_landmarker.task')
    options = vision.HandLandmarkerOptions(base_options=base_options,
                                          num_hands=1)
    detector = vision.HandLandmarker.create_from_options(options)
    image_mp = mp.Image(data=image_np, image_format=mp.ImageFormat.SRGB)
    detection_result = detector.detect(image_mp)
    if len(detection_result.hand_landmarks) == 0:
        return None
    tips = list(range(21))
    tips_px = []
    image_rows, image_cols, _ = image_np.shape
    for tip in tips:
        tip_norm = detection_result.hand_landmarks[0][tip]
        tip_px = _normalized_to_pixel_coordinates(tip_norm.x, tip_norm.y, image_cols, image_rows)
        tips_px.append(tip_px)
    return tips_px


