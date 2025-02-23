Tjenixen Hans! lite galenskaper här i inlämningen, men saker att notera av väde är följande:

Typ alla debug.writeline är Chatgpt genererade, använde dessa när abosolut inget ville funka (Jag hade glömt lägga till något i DI containern...) i övrigt har ChatGpt använt för lite formatering här och där och för att fösöka banta ner lite kod. Se kommentarer i koden
även använt chatgpt till att "rätta" uppgiften denna gången också, men denna gången så blev Chatten så fruktansvärt laggig att den nästan var obrukbar, och man märker snabbt att koden som genereras ju större arbetet blir är helt obrukbar.


Services funkar typ inte alls, fast ändå lite. Tanken var att man checkar checkboxarna i en employee för att ge behörighet att använda de olika services konotra employee i projekten. fick aldrig det att fungera så nu är dom hårdkodade när databasen skapas.
Roller hårdkodas också när databasen skapas.

EstimatedHoures är kopplat till pris, men funkar inte heller riktigt som det ska, tanken var att man skulle få mata in hur många estimerade timmar som skulle gå åt, och därmed öka eller sänka totalpriset. Detta fick jag inte heller att fungera och är istället bara direkt
kopplad till en service. 

Hade också en existensiell kris ang. hur man skulle lägga upp Kriterierna ang ef-core genererad SQL, och manuellt skriven. så nu skrivs Tabellerna manuellt och jag har inte anävnt migration, i efterhand hade detta varit en utmärkt fråga att ställa men det är tydligen lätt
att vara efterklok. 

tack för mig, tjohej
