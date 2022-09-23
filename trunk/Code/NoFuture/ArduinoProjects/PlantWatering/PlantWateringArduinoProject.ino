#include <Wire.h>
#include <hd44780.h>
#include <hd44780ioClass/hd44780_I2Cexp.h>

const int AirValue = 650;   //you need to replace this value with Value_1
const int WaterValue = 310;  //you need to replace this value with Value_2

//this is the percent above which no watering is required
const int NEEDS_WATERING = 22;
const int WATER_SECONDS = 3;

//this could be controlled by a potentiometer 
const int LOOP_DELAY = 2000;

hd44780_I2Cexp lcd;
int counter = 0;
1
void setup() {
  
  // put your setup code here, to run once:
  Serial.begin(9600);

  //LCD 5v
  //LCD SDA -> A4
  //LCD SCL -> A5
  lcd.begin(20, 4);
  lcd.backlight();
  lcd.clear();

  //Capacitive Sensor 3.3v
  //AUOT -> A3
  //VCC -> 3.3v
  //GND -> common ground

  //Relay VCC -> D3
  //Relay COM -> 3.3v
  //Relay NO -> Pump Red
  //Pump Black -> common ground
  pinMode(3, OUTPUT);
  TurnPumpOff();
}

void loop() {

  lcd.clear();

  int smp = GetSoilMoisturePercent();

  WaterPlants(smp, NEEDS_WATERING, WATER_SECONDS*1000);
  
  delay(LOOP_DELAY);
  
  counter++;
}

int GetSoilMoisturePercent(){
  
  //read from the sensor
  int smv = ReadSoilSensor();

  //map raw reading to percentage
  int smp = map(smv, AirValue, WaterValue, 0, 100);
  
  smp = smp >= 100 ? 100 : smp;
  smp = smp <= 0 ? 0 : smp;

  Serial.println(smv);
  Serial.print(smp);
  Serial.println("%");
  
  lcd.setCursor(0, 0);
  lcd.print("Soil Moisture");
  lcd.setCursor(0, 1);
  lcd.print(smp);
  lcd.print(" %, ");
  lcd.print(smv);

  return smp;
}

void WaterPlants(int soilMoisturePercent, int minimumPercentage, int pumpOnInSeconds){
  if(soilMoisturePercent > minimumPercentage){
    return;
  }

  TurnPumpOn();
  delay(pumpOnInSeconds);
  TurnPumpOff();
}

int ReadSoilSensor(){
  return analogRead(A2);
}

void TurnPumpOn(){
  digitalWrite(3, LOW);
  lcd.setCursor(0, 2);
  lcd.print("Relay On ");
}

void TurnPumpOff(){
  digitalWrite(3, HIGH);
  lcd.setCursor(0, 2);
  lcd.print("Relay Off");
}
