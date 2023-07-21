import numpy as np
import cv2
def draw_on_images(image: np.ndarray, tips_px):
    # image: a numpy array in SRGB format
    for tip_px in tips_px:
        cv2.circle(image, tip_px, 15, (255, 0, 0), 2)
    return image
