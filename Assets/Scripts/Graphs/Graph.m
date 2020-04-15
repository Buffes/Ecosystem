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

figure(1)      
p = plot(col1,col2, col1,col4);
legend(p, names)
ylabel('Number of Animals')
xlabel('Time')
figure(2)       
ps = plot(col1,col3, col1,col5);
legend(ps, names)
ylabel('Average Speed')
xlabel('Time')
figure(3)      
phr = plot(col6,col7, col6,col8);
legend(phr, names)
ylabel('Average Hearing Range')
xlabel('Time')
figure(4)
pvr = plot(col9,col10, col9,col11);
legend(pvr, names)
ylabel('Average Vision Range')
xlabel('Time')






