<<<<<<< HEAD
SpeedArray = csvread('SpeedDoc.csv');
col1 = SpeedArray(:,1);
col2 = SpeedArray(:,2);
col3 = SpeedArray(:,3);
col4 = SpeedArray(:,4);
col5 = SpeedArray(:,5);
HearingArray = csvread('HearingDoc.csv');
col6 = HearingArray(:,1);
col7 = HearingArray(:,2);
col8 = HearingArray(:,3);
VisionArray = csvread('VisionDoc.csv');
col9 = VisionArray(:,1);
col10 = VisionArray(:,2);
col11 = VisionArray(:,3);
names = {'lions', 'chickens'};


figure
subplot(2,2,1)      
p = plot(col1,col2, col1,col4);
legend(p, names)
title('#Animals/Time')
subplot(2,2,2)       
ps = plot(col1,col3, col1,col5);
title('Speed/Time')
subplot(2,2,3)      
phr = plot(col6,col7, col6,col8);         
title('HearingRange/Time')
subplot(2,2,4)       
pvr = plot(col9,col10, col9,col11);
title('VisionRange/Time')
=======
Array = csvread('GraphValues.csv');
col1 = Array(:,1);
col2 = Array(:,2);
col3 = Array(:,3);
%col4 = Array(:,4);
%col5 = Array(:,5);
%col6 = Array(:,6);
%col7 = Array(:,7);
%col8 = Array(:,8);
%col9 = Array(:,9);
%names = {'lions', 'chickens'};


%figure
%subplot(2,2,1)      
%p = plot(col1,col2, col1,col3);
%legend(p, names)
%title('#Animals/Time')
%subplot(2,2,2)       
%ps = plot(col1,col4, col1,col7);
%title('Speed/Time')
%subplot(2,2,3)      
%phr = plot(col1,col5, col1,col8);         
%title('HearingRange/Time')
%subplot(2,2,4)       
%pvr = plot(col1,col6, col1,col9);
%title('VisionRange/Time')
>>>>>>> 94b1cb8315da42493377dac2cb74717540a873bc





