names = {'lions', 'chickens'};
SpeedArray = csvread('SpeedDoc.csv');
col1 = SpeedArray(:,1); %time
col2 = SpeedArray(:,2); %lion count
col3 = SpeedArray(:,3); %lion stat
col4 = SpeedArray(:,4); %chicken count
col5 = SpeedArray(:,5); %chicken stat

TempArray = GetValues(col1,col2,col3,col4,col5,SpeedArray);
figure(1)      
p = plot(TempArray(:,1),TempArray(:,2), TempArray(:,1),TempArray(:,4));
legend(p, names)
ylabel('Number of Animals')
xlabel('Time')

figure(2)       
ps = plot(TempArray(:,1),TempArray(:,3), TempArray(:,1),TempArray(:,5));
legend(ps, names)
ylabel('Average Speed')
xlabel('Time')

HearingArray = csvread('HearingDoc.csv');
col1 = HearingArray(:,1);
col2 = HearingArray(:,2);
col3 = HearingArray(:,3);
col4 = HearingArray(:,4);
col5 = HearingArray(:,5);
TempArray = GetValues(col1,col2,col3,col4,col5,HearingArray);

figure(3)      
phr = plot(TempArray(:,1),TempArray(:,3), TempArray(:,1),TempArray(:,5));
legend(phr, names)
ylabel('Average Hearing Range')
xlabel('Time')

VisionArray = csvread('VisionDoc.csv');
col1 = VisionArray(:,1);
col2 = VisionArray(:,2);
col3 = VisionArray(:,3);
col4 = VisionArray(:,4);
col5 = VisionArray(:,5);
TempArray = GetValues(col1, col2, col3, col4, col5, VisionArray);

figure(4)
pvr = plot(TempArray(:,1),TempArray(:,3), TempArray(:,1),TempArray(:,5));
legend(pvr, names)
ylabel('Average Vision Range')
xlabel('Time')

function [TempArray] = GetValues(col1, col2, col3, col4, col5, Array)
   C = unique(Array(1));
   a = col1(1);
   pos = 1;
   val1 = 0;
   val2 = 0;
   val3 = 0;
   val4 = 0;
   TempArray = zeros(C,5);

    for i = 1:1:length(Array)
       
        if col1(i) == a
            val1 = val1 + col2(i); %nr lions
            val2 = val2 + col3(i); %lion stat
            val3 = val3 + col4(i); %nr chickens
            val4 = val4 + col5(i); %chicken stat
        end
        
        if col1(i) ~= a || i == length(Array)
            TempArray(pos,1) = a;
            TempArray(pos,2) = val1;
            TempArray(pos,3) = val2/val1;
            TempArray(pos,4) = val3;
            TempArray(pos,5) = val4/val3;
            pos = pos + 1;
            val1 = col2(i);
            val2 = col3(i);
            val3 = col4(i);
            val4 = col5(i);
        end    
        a = col1(i); 
    end
end
