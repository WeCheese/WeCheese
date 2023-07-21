import numpy as np
import cv2
from hand_landmarker import detect
from draw_utils import draw_on_images

cap = cv2.VideoCapture(0)
if not cap.isOpened():
    print("Cannot open camera")
    exit()
while True:
    # Capture frame-by-frame
    ret, frame = cap.read()
    # if frame is read correctly ret is True
    if not ret:
        print("Can't receive frame (stream end?). Exiting ...")
        break
    # Our operations on the frame come here
    image_np = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
    tip_pxs = detect(image_np)
    if tip_pxs is not None:
        image_rgb = draw_on_images(image_np, tip_pxs)
        image_gbr = cv2.cvtColor(image_rgb, cv2.COLOR_RGB2BGR)
        # Display the resulting frame
        cv2.imshow('frame', image_gbr)
    else:
        cv2.imshow('frame', frame)
    if cv2.waitKey(1) == ord('q'):
        break
# When everything done, release the capture
cap.release()
cv2.destroyAllWindows()