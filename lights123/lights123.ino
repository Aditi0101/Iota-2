int flag_light = 0;
int flag_fans = 0;
int flag_door = 0;
char s='w';
void setup()
{
  Serial.begin(9600);
  pinMode(8,OUTPUT);
  pinMode(9,OUTPUT);
  pinMode(10,OUTPUT);
  pinMode(11,OUTPUT);
  pinMode(7,INPUT);
  pinMode(6,INPUT);
  pinMode(4,INPUT);
  //digitalWrite(A1,LOW);
}

void loop()
{
  //while(1)
  //Serial.println('Start');
  
  //while(Serial.available() != 0)
  {
    s =Serial.read() ;
    //int a =  analogRead (A1);
     if(s == 'e' || digitalRead(7) == LOW)
    {
        if(flag_light == 0)
        {
           digitalWrite(8,HIGH);
        }
      else 
      {
         digitalWrite(8,LOW);
      }
      
      flag_light = (flag_light + 1)%2;
      //Serial.println(a);
      Serial.write('y');
      delay(1000);
    }
    
    if(s == 'f' || digitalRead(6) == LOW)
    {
        if(flag_fans == 0)
        {
           digitalWrite(9,HIGH);
        }
      else 
      {
         digitalWrite(9,LOW);
      }
      
      flag_fans = (flag_fans + 1)%2;
      //Serial.println(a);
      Serial.write('y');
      delay(1000);
    }
    
   // if(s == 'd' || digitalRead(4) == LOW)

    {
        if(flag_door == 1)
        {
           digitalWrite(10,HIGH);
           digitalWrite(11,LOW);
           delay(450);
           digitalWrite(10,LOW);
           digitalWrite(11,LOW);
        }
      else 
      {
         digitalWrite(10,LOW);
         digitalWrite(11,HIGH);
         delay(450);
           digitalWrite(10,LOW);
           digitalWrite(11,LOW);
      }
      
      flag_door = (flag_door + 1)%2;
      //Serial.println(a);
      Serial.write('y');
      delay(1000);
    }
  }
}

void serialEvent()
{
}

