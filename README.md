[![Board Status](https://dev.azure.com/santapone/275cd44b-4a45-4978-92cf-c96f59c14bec/73ea5cf8-3ebb-4dba-8c57-95c3427ce42a/_apis/work/boardbadge/f2579203-5dc9-4a6c-8de6-57056d0bb37f)](https://dev.azure.com/santapone/275cd44b-4a45-4978-92cf-c96f59c14bec/_boards/board/t/73ea5cf8-3ebb-4dba-8c57-95c3427ce42a/Microsoft.RequirementCategory)
# SantaPone Central

## What is SantaPone Central?
SantaPone Central is a structured web application to help with the management, assignment, and correspondance for the /mlp/ Secret Santa event, allowing for admins and users to have a safe and secure place to see their assignments, keep in contact, and be notified when things happen without the need for an email string for each and every Anon in the event.

Dev environment (Active testing and development): https://dev-santaponecentral.azurewebsites.net/
Production enviroment (Feature complete updates and where the event will take place): https://www.santaponecentral.net/


### For the more nerdy overview of the site:
SantaPone Central is a full stack application with an ASP.NET Web API backend, interfacing Auth0 for a stable and secure role-based management system, and Twilo Sendgrid as a privately used Azure service of its own to have a secure way to deliver notifications and other information to users. The front end is an auth0 manged Angular Single-Page Application itself that utilizes best practice to make for a simple and effective way for clients and event admins to manage and contribute to the event.

### Costs
All the development work itself is something I've been doing as a kindness to the event organizers and all you wonderful Anon's to be able to have a nice way to help everyone participate in bringing joy and ponies to the world. This includes also footing the bill of app platform hosting for the better part of this last year. Being that this is the case, as things move towards the actual event, costs for hosting will go up, and while I don't forsee myself being unable to keep the hosting up, donations and help with the cost of platform hosting are greatly appreciated, and the Ko-Fi link to help out with that is right here if you'd like to help out in that respect!

https://ko-fi.com/santaponecentraldev

## Service Infrastructure

### Back End
There are namely 3 parts to the backend infrastructure; the data service, the email service, and the auth0 service. All of these work together to ensure that data is valid, secure, and stateless. A privately hosted Twilo Sendgrid solution is used to ensure that emails from the API are uniform, and specific to events, assignments, and correspondence to notify users of assignments, correspondence, or any other things that need to be sent, such as password reset requests. The Auth0 service itself is used by utilizing the Auth0 management API itself, ensuring data security and proper credential handling. Being that it as API running on it's own domain, requests are only allowed to come from this site, and the Auth0 tenants used in the data security layer. This specifically has been rigorously tested for fidelity to ensure data security management.

### Front End
The front end site you see before you was made with the Angular framework to deliver a nice, Single-Page Application web experience. I personally am a huge fan of Angular and the way type-safety and services in-application can be accessed and handled. SantaPone Central is hosted on its own app platform service separate from the API to ensure that data calls and services are exclusively handled on the API layer, and features data protection layers and services by Auth0 to manage user authorization and requests.

### Sendgrid
Twilo Sendgrid is one of the services hosted privately within Azure. Rather simply, it is the service that allows the API to send secure emails about notifications and correspondence in general.

### Hosting
The hosting of all of these services are hosted within Azure. If you are not familiar with service architectures, in short, similar services would be Amazon Web Services and CloudFlare. These are secure and expandable cloud computing solutions that allow for a safe and consistent running environment. I use Azure specifically because it is what I am most familiar with, and it interfaces well with a lot of the other Microsoft-based services and frameworks like SendGrid, or ASP.NET in general.

## Privacy
Getting on this one right away, security was the topmost priority in making this site. Every single piece of data that can or will be ever accessed is locked and vigorously checked behind a security layer that Auth0 provides. I am aware that giving out any information, no matter the platform, is an absolutely HUGE part of trust, which is why I chose Auth0 as the security and data management solution.

### What exactly is Auth0?**
Auth0, in short is a security service that handles client login information and security

### How secure is it?
To put it mildly, this is a security platform that AMD, Atlassian, and BlueCross BlueShield themeselves use. It provides a secure way to attach validation to requests that are made with roles that define information about a user, allows for a safe API to manage password changes, and ensures that quite literally nothing gets past the security layer that shouldn't.

### Passwords
For the sake of security, there are literally no passwords that are or ever will be stored on the database for SantaPone Central. All user credentials and security information is handled exclusively by a privately paid and managed Auth0 tenant by yours truly to make sure every single one of your data is secure.

### Is the event going to be any different?
Absolutely not! SantaPone Central was made with the intent to make sure that the event itself functions exactly as the event has in the past, with a signup form, approvals from admins, and getting assignments.

### B-But the board??
The goal of this whole endevor is to have a site to help the event organizers manage the event, and for you to communicate directly to them about assignments, not to offload any activity from the board. None of what the site can do takes away from the wonderful threads there, and it is more than encouraged to still use the board for all the /SS/ activities, screenshots, and other things that go on!

## Data

### What data is stored?
The goal of SantaPone Central is to literally keep the same process to the /SS/ event in a managed, easy-to-use way. That being said, the only things that are stored on the database is the information that has always been asked for. Your names, emails, address, and survey questions are the only things that are asked for upon signing up, and are vigorously and strictly locked behind auth0 for peace of mind and protection.

### Who can access my data?
Auth0 allows for role-based authorization. That being said, the only people who can view your data are the admins, and the people who are set to send to yourself as an assignment. The API has been carefully developed to make sure that the profile responses to users only gives the necessary data to that user, being the information SantaPone has always given in emails in past events.

### What about after the event?
After the Secret Santa Event, I plan to remove all the user data once SantaPone is able to download the tables needed for her own archival purposes in terms of knowing who grinched, or any other information needed. Auth0 Accounts will also be deleted, and the data service will be brought offline until the need for the next event.
