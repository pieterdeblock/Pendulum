
## Feedback Applied Programming

### Opgave 2: Pendulum Wave

#### Algemeen

#### Architectuur (15%)

***Modulair, meerlagenmodel***

- [x] *Meerlagenmodel via mappen of klassebibliotheken*
- [x] *Dependency injection*
- [x] *Gebruik  MVVM Design pattern*

***'Separation of concern'***

- [ ] *Domein-logica beperkt tot logische laag*
- [x] *Logische laag onafhankelijk van presentatielaag*

* Je simulation loop wordt gestuurd vanuit de presentatielaag. Die zou helemaal autonoom in je wereldmodel moeten staan.


#### Programmeerstijl, Kwaliteit van de code (10%)

***Naamgeving***

- [x] *Naamgeving volgens C# conventie*
- [x] *Zinvolle, duidelijke namen*

* Je volgt niet altijd de conventies voor naamgeving in C#.

***Korte methodes***

- [x] *maximale lengte ~20 lijnen*

***Programmeerstijl***

- [x] *Layout code*
- [ ] *Correct gebruik commentaar*
- [x] *Algemene programmeerstijl*

* Verwijder code-blokken die in commentaar staan (en niet meer nodig zijn).


#### User interface, functionaliteit, UX (15%) 

***Ergonomie***

- [ ] *Layout UI*
- [ ] *estetische weergave* 
- [ ] *Goede UX*

* De UI ziet er slorif uit en onafgewerkt.
* Er is geen mogelijkheid om de (hulp)assen te evrbergen: die zijn storend in het beeld.

***functionaliteit***

- [x] *Goede weergave view met controllerbare camera*
- [ ] *Goede weergave 'Bottom' view*
- [x] *Weergave numerieke resultaten*
- [x] *Instelbaar aantal slingers*
- [x] *Instelbare kleurenweergave*
- [x] *Start, Pauze, Reset*

#### Goede werking, snelheid, bugs (25%)

- [x] *Correcte simulatie slinger (o.a. formules)*
- [ ] *Correcte berekening lengte van de slingers (o.a. formule)*
- [ ] *Maximale simulatiefrekwentie*
- [x] *Realistische renderfrekwentie*

* Je 'simulation loop' zou met een maximale snelheid uitgevoerd moeten worden (in tegenstelling tot de 'render loop'). Bij jou is dat één loop die veel te traag loopt voor een goede simulatie.
* `BerekenHoek`komt me wel heel erg bekend voor?
* De gebruikte lengtes van je slingers zijn niet bruikbaar om een 'wave' effect te krijgen.

***Juiste werking***

- [x] *Goede werking*

***Snelheid, efficiëntie, concurrency***

- [ ] *Zinvol gebruik concurrency*
- [x] *Efficiënte berekeningen*

* `UpdateSlinger` en `TransformSlingers` doen exact hetzelfde. Waarom roep je beiden op in je renderloop?
* Waarom start je in `dispatcherTimer_Tick` - `Help` nog een keer een Task? dat vertraagt vooral de werking en heeft geen meerwaarde.

***Bugs***

- [x] *Geen bugs*

#### Installeerbare package voor distributie (10%)

- [x] *Installable package beschikbaar in repo*

* Je hebt geen eigen iconen of images toegevoegd aan je installable package.

#### Correct gebruik GIT (10%)

- [ ] *Gebruik 'atomaire' commits*
- [x] *zinvolle commit messages*

* Je commits zijn niet (altijd) atomair: je hebt heel wat commits waarin je meerdere losstaande wijzigingen hebt gedaan.


#### Rapportering (15%)

- [ ] *Structuur*
- [ ] *Volledigheid*
- [ ] *Technische diepgang*
- [x] *Professionele stijl*

* Gebruik geen 'ik'-vorm in een technishc verslag of rapport.
* Je verslag is vooral een inventaris van een aantal methodes maar je blijft erg oppervlakkig en geeft nauwelijks technische uitleg over de mechansiemn en berekeningen die gebruikt worden in je code.