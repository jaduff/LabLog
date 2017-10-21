# LabLog
A web app for logging physical users and damage in computer labs

## Context
This project is an attempt to increase accountability of students using shared computer labs in schools. To reduce malicious damage, administrators need to know who (physically) was responsible for that workstation (may not have logged in as themselves?).

## UI Wireframes
Interactive wireframes generated from Adobe Experience Design (beta) https://xd.adobe.com/view/d8a50ef1-51f1-40da-9588-874151c18c0e/


## To Do
* Optimistic locking on event creation
* When an event is used to update the read model, it should compare version numbers to ensure consistency.
* Implement Computer View
* Implement DamageModel
* Implement ComputerUserModel
* Implement Teacher controller and views