# WooliesXCodingChallenge
Store the back-end artifacts for the WooliesX coding challenge

## Serverless (FaaS) vs WebApp Deployments
I understand that in certain cases FaaS can be more useful as we only pay for the time the function is running and it appears that all the exercises do not require anything that a Backend supports (IE DBs) which would mean that a function may be more suited. However, for the purposes of keeping the assessment code together and simplicity, the endpoints are located on one resource. (See section on **Microservices**)

The endpoint is set up at https://wooliesxcodingchallenge20200929124301.azurewebsites.net/api/<EXCERISE_ENDPOINT> and will be running as long as the account has (free) credit on it, mitigating delays for cold starts. I would suspect that there would not be too much difference between deploying as WebApps or function (at this stage), just different base URLs for each of the exercise endpoints on an Azure Function based implementation

## Assumptions
- DBs are not needed for servicing requests
- Requirements was to build an HTTP API. It is assumed that this means we are contacting a (RESTful) HTTP API method/s and not that we are contacting port 80 (i.e. GET http://)
- In the interest of time under assessment conditions it is assumed that consumers of the /trolleyTotal API is not intentionally trying to break it (For what I would have done to support/account for Fault Tolerance, please see the API Fault Tolerance Section)
- Secret used to contact the resource URL (including the applicant specific token) does not need to be securely store (At least for the first implementation under assessment conditions. Please see Token and URL storage for more details and how I would have done it)


## Execrise Though Processess
### Exercise 1
**Input:** GET request to `https://[FQDN]/api/user`\
**Output:** { "name": "<NAME>", "token": "<TOKEN>" }

##### Thoughts
It appears to me that there isn't a need for a DB from the specification (as we are just returning a constant `user` (singular)), however just as a precaution I have created a service for it just in case. I suspect that in a normal environment we would need to communicate to a DB to authenticate users, hence the service\
In theory, one could just hard code it directly in the controller

### Exercise 2
**Input:** GET request to `https://[FQDN]/api/sort?sortOption=[VALUE]`\
**Output:** The response from <https://<FQDN>/resources/products> and <https://<FQDN>/resources/shopperHistory> ordered by the query param supplied
  
##### Thoughts
The idea here was to use the IComparer/Comparer Classes and Interfaces to define the order of which to sort the products by, which is what I have done.
I have use Dictionaries where I can when I need constant-time access

### Exercise 3
**Input:** POST request to `https://[FQDN]/api/trolleyTotal`\
**Output:** Lowest cost for that trolley
  
##### Thoughts
I thought this problem was very interesting :) (On later passes I noticed that this could leverage an interpreter like pattern to evaluate the lowest trolley price)\
Initially, my thoughts were to start with the base case (where I don't have any specials) where the lowest total is just the sum of all the products in the cart. Then I considered one `special`, followed by two then three to which I came to the realisation that this problem is about which `special` to apply when.

For simplicity and as a (moderate/first solution) a `greedy` algorithm was used, where at each step we apply the most cost-saving `Special` on the Trolley. 

The steps essentially look like this:
- Calculate the savings of each `Special`
- Order `Specials` by ones that give us the most savings first
- While I can still apply a `Special`, \
-- Apply the `Special` and remove those items from the trolley and add the `Special`'s price to the trolley total\
-- Repeat until there is no `Specials` that I can apply (this can be true for 0 or more items in the cart)
- Add up all the remain items in the trolley that are not bound by a `Special` and total the trolley's total
- Return the trolleyTotal

On multiple passes analysing the algorithm, there was the possibility that a lesser total could be reached if we were consistently re-evaluating the sum of `specials` but that would very quickly become a difficult problem. Similar to the branching paths on the state tree of the travelling salesman problem, there would be different nodes one could arrive on in the state tree should we choose to apply `special 1` instead of `special 2`, of which each one of those would have another set `specials` to apply, moving us further down a level on the state tree (ie applying `Special 1` then `Special 2` could be different from applying `Special 2` then `Special 1`, and each would be on different branches in the state tree with their own children nodes to traverse). This could very quickly exceed memory limits so one would probably start to look at more efficient ways of exploring and storing state trees information, like `Branch & Bound Depth First Search` and using a byte[] instead of strings (representation of the state tree)

# Room for Improvement
## Logging and Tests
Currently, this project does not have logging or unit/integration tests, Given more time TDD could have been performed and different levels of logging could have also been set up, but due to the time constraints, I was not able to do this.

## Token and URL Storage
Under normal circumstances, the resource URL would have been stored in something like Appsettings.json and retrieved from there. Like-wise with the participant `token` on Azure KeyVault. However, due to time constraints and connectivity issue I had with the KeyVault, they were stored in the repository

## API Fault Tolerance
I would have liked to introduce more error checking on the /trolleyTotal API. Null checks, Negative values, Use of products in specials with no price, Items in the trolley which do not have product listing for it, etc. At the time of development, I was concerned with solving the immediate issues hence the more traditional approach to development. Under normal circumstances I would have like to plan out the inputs and outputs then develop the endpoints using TDD to cover the previously mentioned cases. (Returning different 4xx responses).

In an ideal world, I would also like to have added a schema validator on each HTTP handler returning code 409 in the event that the payload/request is malformed.

## Branching and Merge Strategies
Under normal development environment, I would **NEVER** push directly onto master, instead creating PRs for each feature change I make (that's why there is a `[JOB_NUMBER]` tag on each of the feature commits).

## Microservices
For the purposes of the assessment, I have put everything in one repository, as it makes it easier to see all the artefacts together. In practice (if I didn't use functions) I would have split each exercise into its own domain (each domain has their own service as they are all touching different domains, Ordering, Browsing, Authentication & Authorisation), ensuring that the only time resources are allocated is when that resource needs to be used (i.e. we do not have idle resources for the /api/user endpoint if the system upscales to accommodate high load for /api/trolleyTotal). I would then have an API Gateway (and potentially CDN's) out the front to service different requests, help performance via caching of static data and provide some help with DDOS attack mitigation.

## Final Thoughts
I quite enjoyed this exercise as it gave me the opportunity to test my knowledge of the full process of deploying an application.\
I know that there may be decisions that people may disagree with and I happy to discuss them why I made them and have my thought and thought processes changed!
