# MemorySpil

![MemorySpil]

**Team5 – UCL Odense, Computer Science AP**  

MemorySpil er et klassisk kortspil, hvor spilleren skal finde matchende par blandt 16 kort. Spillet er udviklet i C# ved brug af WPF og følger MVVM-arkitekturen samt repository pattern for datahåndtering.  

## Funktioner
- 16 kort, hvoraf to vendes ad gangen  
- Matchende kort registreres automatisk  
- Tæl forsøg og tid for at skabe konkurrence  
- Spillernavn registreres, og topscorer gemmes mellem spil (filbaseret)  
- Grafisk brugerflade med WPF  

## Teknologi
- **Sprog:** C#  
- **UI:** WPF  
- **Arkitektur:** MVVM  
- **Datahåndtering:** Repository pattern og filhåndtering  

## Spilmekanik
1. Spilleren indtaster sit navn  
2. Spilleren vender to kort ad gangen  
3. Hvis kortene matcher, bliver de stående åbne, ellers vendes de tilbage  
4. Antal forsøg og tid registreres løbende  
5. Når alle par er fundet, vises resultatet og sammenlignes med tidligere topscore  

## Filstruktur
- **Model:** Definerer spillets data (kort, spiller, score)  
- **View:** WPF-brugerflade  
- **ViewModel:** Binder data til UI og håndterer logik  
- **Repository:** Håndterer læsning og skrivning af topscore til fil  


## Installation og brug
1. Åbn løsningen i Visual Studio
2. Kør projektet og indtast dit navn for at starte spillet

## Bidrag
Dette projekt er udviklet af **Team5** som en del af Computer Science AP på UCL Odense 2nd semester 1st år, 2025.

## License
Dette projekt er open source og kan frit anvendes og videreudvikles.
