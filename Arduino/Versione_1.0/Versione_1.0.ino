void setup(){
  Serial.begin(9600);
  pinMode(LED_BUILTIN, OUTPUT);
}

void loop(){
  String parola = leggiParola();
  //Serial.println(parola);
  //Serial.write("picio");
  if (parola == "Suona"){
    digitalWrite(LED_BUILTIN, HIGH);
    Serial.println("Sto illuminando");
    delay(5000);
    digitalWrite(LED_BUILTIN, LOW);
  }
  delay(50);
}

String leggiParola(){
  String finale;
  while(Serial.available()){
    char c = Serial.read();
    if (c == ';')
      return finale;
    else{
      finale+=c;
    }
    delay(10);
  }
}
