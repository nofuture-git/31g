# NoFuture.Rand
---

This is a collection projects for generating test data at random.

---

##### NoFuture.Rand.Core
The base assembly on which the other _NoFuture.Rand_ projects depend.  Suited for generation of random strings, dates, numbers along with other random concepts like probability, coin-toss, dice rolls, etc.

##### NoFuture.Rand.Sp
A finance assembly for the random generation of loans, tradelines, transactions, checking\credit cards and other money-related data-forms.

##### NoFuture.Rand.Domus
A project for the generation of personal data like names, spouses, children and parents.  This project also ties in the other random types to create random persons with addresses, personalities, phone-numbers, bank accounts, vehicles and wealth, etc.

##### NoFuture.Rand.Pneuma
A project for generation of personality data based on the trait approach (a.k.a. Big-5).  This is useful to attach to other random generators to get a more colorful output.

##### NoFuture.Rand.Tele
A project for random telephony data like phone numbers, email addresses, URIs, host-names, etc.

##### NoFuture.Rand.Geo
A project for random postal address data like zip-codes, city names, street and PO box data, etc.

##### NoFuture.Rand.Org
A project for random data defined by external organizations.  Contains methods for random professions and economic sectors.

##### NoFuture.Rand.Gov
A project for random data related to government entities along with other forms of data published by the government.  This project includes methods for random SSN's, Birth and Death certs, Driver's License, VIN numbers (vehicles), etc.

##### NoFuture.Rand.Edu
A project for random education data.  This project has methods to get random Universities and High Schools.

##### NoFuture.Rand.Opes
A project for the generation of random personal wealth data.  This project includes random methods for income, employment, assets and deductions.  Its data-form is based on various US financial affidavits.

##### NoFuture.Rand.Com
A project for the generation of random company and business-entity data.

##### NoFuture.Rand.Exo
This project is a used to parse and assign data read from external sources into the entities defined in _NoFuture.Rand.Com_.  

##### NoFuture.Rand.Src
This is an _F#_ project which is used to generate the consolidated government data.  This project deals with the big-data from the BEA and BLS.

### Summary
The various random values generated are based on real data and will therefore look _real_ to a typical person or machine.  For example, random VIN numbers will be based on real values and will have valid check-digits; however, its only use is for testing or just fun.  Unit-tests will sometimes run red because, well, its random.