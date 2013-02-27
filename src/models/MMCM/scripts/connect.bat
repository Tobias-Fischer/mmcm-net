
yarp connect /mmcm_head/hierarchical/predicted:o /mmcm_peripersonal/head/real:i
yarp connect /mmcm_peripersonal/head/predicted:o /mmcm_head/hierarchical/real:i


yarp connect /mmcm_left_arm/hierarchical/predicted:o /mmcm_peripersonal/left_arm/real:i
yarp connect /mmcm_peripersonal/left_arm/predicted:o /mmcm_left_arm/hierarchical/real:i 

yarp connect /mmcm_torso/hierarchical/predicted:o /mmcm_peripersonal/torso/real:i
yarp connect /mmcm_peripersonal/torso/predicted:o /mmcm_torso/hierarchical/real:i 

yarp connect /mmcm_vision/hierarchical/predicted:o /mmcm_peripersonal/vision/real:i
yarp connect /mmcm_peripersonal/vision/predicted:o /mmcm_vision/hierarchical/real:i 