#include <FastLED.h>

/*
FastLED.h @ line 587 CRGB is defined
*/

FASTLED_USING_NAMESPACE

#define DATA_PIN    3
//#define CLK_PIN   4
#define LED_TYPE    WS2811
#define COLOR_ORDER GRB
#define NUM_LEDS    52
#define BUTTON_PIN  7
#define DIMMER_PIN  A2
#define DF_COLOR    255
CRGB leds[NUM_LEDS];

#define BRIGHTNESS          20
#define FRAMES_PER_SECOND  120

void setup() {
  delay(500); // 3 second delay for recovery
  Serial.begin(57600);
  // tell FastLED about the LED strip configuration
  FastLED.addLeds<LED_TYPE,DATA_PIN,COLOR_ORDER>(leds, NUM_LEDS).setCorrection(TypicalLEDStrip);
  //FastLED.addLeds<LED_TYPE,DATA_PIN,CLK_PIN,COLOR_ORDER>(leds, NUM_LEDS).setCorrection(TypicalLEDStrip);

  // set master brightness control
  FastLED.setBrightness(BRIGHTNESS);

  //init button as an input with default pull-up val
  pinMode(BUTTON_PIN, INPUT_PULLUP);

  //init the color to default white
  for( int i = 0; i < NUM_LEDS; i++){
    leds[i].setRGB(DF_COLOR,DF_COLOR,DF_COLOR);
  }
  FastLED.show();
}
  
void loop()
{
  if(digitalRead(BUTTON_PIN) == LOW){
    SetLedsToRandomColor();
  }
  int sensorValue = analogRead(DIMMER_PIN);
  int mappedSensorValue = map(sensorValue, 0, 1023, 0, 100);
  //Serial.println(mappedSensorValue);
  FastLED.setBrightness(mappedSensorValue);
  FastLED.show();
  delay(250);
}

void SetLedsToRandomColor(){
  int red = rand() % 255;
  int green = rand() % 255;
  int blue = rand() % 255;
  for( int i = 0; i < NUM_LEDS; i++){
    leds[i].setRGB(red, green, blue);
  }
}
