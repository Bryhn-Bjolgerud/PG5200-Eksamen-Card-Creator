# PG5200 Tools Programmering

# Eksamen Høst 2020

Contributors: [makka1998](https://github.com/makka1998) & [BryhnBjolgerud](https://github.com/Bryhn-Bjolgerud)

## Innledning

Vi har fått i oppgave å lage en card editor for et spill som f.eks Hearthstone. Årsaken
til at vi ønsker å lage en applikasjonen som denne, er at spilldesignere skal kunne
effektivt masseprodusere potensielle kort til spillet sitt. Krav til applikasjonen er at det
har blitt programmert i C#, med bruk av WPF - Windows Presentation Foundation -,
benyttet enten Sql server eller SQLite for database og bruken av en Json
serialiserer. Det er mulig å lage WPF applikasjoner i andre IDEer en Visual Studio,
men siden et annet krav var at løsningen skulle kunne kjøres og kompileres fra
Visual Studio 2019, valgte vi å bruke det.

Vi var veldig fornøyd med oppgaven. Det var deilig å ikke måtte lage enda en
console applikasjon, men noe grafisk med et ordentlig bruksområde. Vi har begge to
mye erfaring med spillet Hearthstone så det falt oss veldig naturlig inn å lage vår
editor rettet mot det. Når man jobber med noe hvor man har en faktisk interesse av
resultatet utenom bare karakteren, blir prosessen ofte mye morsommere.

Det er en selvfølge at parallelt med å legge til ny funksjonalitet i programmet vårt,
økte kvaliteten på det. Men dette var den første oppgaven vi har jobbet på hvor
sluttproduktet var noe så “ordentlig”. De teknikkene/funksjonalitetene som vi var
påkrevd å bruke, gjorde at vi for første gang følte oss som et snev av ordentlig
utviklere. Det er ikke å legge skjul på at vi er stolt av resultatet vårt.


## Setup for å kjøre programmet

For å kunne kjøre løsningen vår så trenger man Visual Studio 2019 og
“Workload’en” .NET desktop development(Sirklet på bildet nedenfor). For å få tak i
workloaden kan man åpne visual studio installer, gå inn på modify og huke av på at
man ønsker den, og så installere den.

![Workload](https://github.com/Bryhn-Bjolgerud/images/blob/main/toolsprogreadmebilde1.png)

Når man har Visual Studio kan man åpne .sln filen for prosjektet vårt. Det vil åpne
opp løsningen vår. Da kan man trykke på ​ _Build → Build_ ​ ​ _Solution_ ​ fra toolbaren for å
kompilere løsningen. Så kan man trykke på ​ _Start_ ​for å kjøre den.

Første gang man kjører programmet så vil den opprette en database fil, Cards.db i
“Documents” mappen på den tilhørende datamaskinen. Hvis denne filen eksisterer
allerede - navnet er ikke særlig kreativt - så vil den bli brukt fremfor at det opprettes
en ny en. Da vil ikke løsningen vår fungere med tanke på at de databasene mest
sannsynlig ikke er satt opp likt. Det kan være relevant å sjekke om det finnes en slik
fil på forhånd og fjerne/endre navn på den før man kjører programmet vårt for å
garantere at løsningen vår vil fungere.

[hobbit-hole](https://en.wikipedia.org/wiki/Hobbit#Lifestyle
