import os
import cv2
from hand_landmarker import detect
from draw_utils import draw_on_images

if __name__ == "__main__":
    files = os.listdir("test_images")
    for filename in files:
        image_bgr = cv2.imread(f"test_images/{filename}")
        image_np = cv2.cvtColor(image_bgr, cv2.COLOR_BGR2RGB)
        tip_pxs = detect(image_np=image_np)
        image_show = draw_on_images(image_np, tip_pxs)
        image_bgr_show = cv2.cvtColor(image_show, cv2.COLOR_RGB2BGR)
        cv2.imshow("annotated", image_bgr_show)
        cv2.waitKey(0)