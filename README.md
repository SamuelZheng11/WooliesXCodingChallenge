# WooliesXCodingChallenge
Store the back-end for the WooliesX coding challenge

## Serverless (FaaS) vs WebApp Deployments
I understand that in certain cases FaaS can be more useful as we only pay for the time the function is running and in this specific case all the HTTP endpoints do not require a database which would mean that a function may be more suited. However, for the purposes of keeping the assessment code together and simplicity, the endpoints are located on one resource

The endpoint is setup at `https://wooliesxcodingchallenge20200929124301.azurewebsites.net/api/<EXCERISE_ENDPOINT>` and will be running as long as the account has (free) credit on it midigating delays for `cold starts`. I would suspect that there would not be too much difference between deploying as webapps or function (at this stage), just different base urls for each of the execise endpoints on a Azure Function based implementation

## Execrise Though Processess
### Exercise 1
**Input:** GET request to https://[FQDN]/api/user\
**Output:** { "name": "<NAME>", "token": "<TOKEN>"}

##### Thoughts
It appears to me that there isn't a need for a DB from the specification (as we are just returning a constant `user` (singular)), however just as a precausion I have created a service for it just in case\
In theory one could just hard code it directly in the controller

### Exercise 2
**Input:** GET request to https://[FQDN]/api/sort?sortOption=[VALUE]\
**Output:** The response from <https://<FQDN>/resources/products> and <https://<FQDN>/resources/shopperHistory> ordered by the query param supplied
  
##### Thoughts
The idea here was to use the IComparer/Comparer Classes and Interfaces to define the order of which to sort the products by, which is what I have done.
I have use Dictionaries where I can when I need constant time access

### Exercise 3
**Input:** POST request to https://[FQDN]/api/trolleyTotal\
**Output:** Lowest cost for that trolley
  
##### Thoughts
I thought this problem was very interesting :) (On later passes I noticed that this could leverage an interperter like pattern to evaluate the lowest trolley price)\
Initially my thoughs was to start with the base case (where I dont have any specials) where the lowest total is just the sum of all the products in cart. Then I considered one `special`, followed by 2 then 3 to which I came to the realisation that this problem is about which `speical` to apply when.

For simplicty and as a (moderate/first solution) a `greedy` algorithm was used, where at each step we apply the most cost saving `Special` on the Trolley. 

The steps essentially looks like this:
- Calculate the savings of each `Special`
- Order `Specials` by ones that gives us the most savings first
- Look through the list to see if any can be applied 
- If they can add the `Special` price on the total and repeat
- If not then add up all the remain items and total the trolley amount

On multiple passes analysing the algorithm there was the possibility that a lesser total could be reached if we were consistently re-evaluating the sum of `specials` but that would very quickly become a difficult problem. Similar to the branching paths on the state tree of the traveling salesman problem, there would be multiple different states on the state tree should we choose to apply `special 1` instead of `special 2`, of which each one of those would have another state tree of `specials` to apply. This could very quickly exceed memory limits so one would probably start to look at more efficent ways of exploring and storing state trees information, like `Branch & Bound Depth First Search` and using a byte[] instead of strings (representation of the state tree)

## Logging and Tests
Currenty this project does not have logging or unit/integration tests, Given more time TDD could have been perfomred and different levels of logging could have also been setup, but due to the time contraints I was not able to do this.

## Token and URL storage
Under normal circumstances the resource URL would have been stored in something like Appsettings.json and retrieved from there. Like-wise with the participant `token` on Azure KeyVault. However due to time constraints and connectivity issue I had with the KeyVault, they were stored in the repository

## Final Thoughts
I quite enjoyed this exercise as it gave me the opportunity to test my knowledge of the full process of deploying an application.\
I know that there may be decisions that people may disagree with and I happy to discuss them why I made them and have my thought and thought processes changed!

