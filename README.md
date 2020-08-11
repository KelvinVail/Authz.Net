# Authz.Net
Role-based access controls (RBAC) for user-to-system and system-to-system permissions management.

Initially this project is just an experiment into implementing a centralized permission management system.

### Background
There are many applications, each with their own role based permissions to manage.  There are many organisations, each with their own users (identities) to manage.  For any given application, management of identities and roles is delegated to an admin in a given organisation.

### Management Roles
#### Global Admin 
Can register new applications and assign app owners and org admins.\
Has access to all application, organisation and identity details.\
Can perform all actions of the following management roles.

#### Application Owner
Can delegate org admins to administer the app they own.\
Can register new permission policies for the apps they own.\
Has access to all organisation and identity details from within the app they own.\
Does not have access to details of identities that are not assigned roles within the app they own.

#### Organisation Admin
Can assign identities from within their organisation to roles in the app they are allowed to administer.\
Has access to all identity details from within their organisation.\
Does not have access to any app details of apps they are not assigned to administer.

### Identity States
#### Registered
Any authenticated and verified identity that exists within the AuthZ management system.

#### Unverified
An authenticated identity that has not had their account and email verified.

#### Suspended
An authenticated identity that has been flagged as suspended.

#### Anonymous
Any user that has not been authenticated and/or registered.

### Usage
#### Initalize a factory
Pass a bearer token into the factory so that it can determin who you are and what management role you hold.  When you call ```AuthZFactory.GetInstance(token);``` an instance of AuthZ will be created with all the appropriate actions you are allowed to perform.
```
new AuthZFactory(IRepository, etc...)
Session AuthZFactory.GetSession(token);
Session AuthZFactory.GetSession(); // If anonymous.
```

#### Indentity Management
##### Get identities
```
IEnumerable<Identity> Session.Identities();
IEnumerable<Identity> Session.Identities().Where(identity => identity.FirstName == "Katie");
IEnumerable<Identity> Session.Identities().Where(identity => identity.IsAppOwner);
```
A Global Admin can get all identities.\
An App Owner can get all identities from with the app they own.\
An Org Admin can get all identities from within their organisation.\
Any other authenticated identity can only get themselves.\
No one else can get identities.

##### Get an Identity
```
Identity Session.Identity(identityId);
```
A Global Admin can get any identity.\
An App Owner can get any identity from within the app they own.\
An Org Admin can get any identity from within their organitasion.\
Any other authenticated identity can only get themselves.\
No one else can get an identity.

##### Get logged in identity
```
Indentity Session.LoggedInIdentity();
```
Any logged in identity can get themselves.

##### Register an Identity
```
void Session.Register(registerIdentityRequest);
```
Any authenticatable identity can be registered.

##### Delete an Identity
Identities are first suspended and then soft deleted after a specified delay.
```
void Session.Identity(identityId).SuspendThenDelete(TimeSpan deleteDelay);
```
A Global Admin can delete any identity (including themselves).\
No one else can delete an identity.

##### Suspend an Identity
```
void Session.Identity(identityId).Suspend();
void Session.Identity(identityId).Reinstate();
void Session.Identity(identityId).SuspendAndResendValidationEmail();
```
A Global Admin can suspend/reinstate any identity.\
An App Owner can suspend/reinstate any identity from the app they own.  They cannot suspend then resend validation email as this would suspend the identity from all apps.\
An Org Admin can suspend/reinstate any identity from within their organisation.\
No one else can suspend/reinstate identities.

```SuspendAndResendValidationEmail()``` suspends an identity until their email has beed validated or revalidated.

Finer grain suspension can be performed like so:
```
void Session.App(appId).Identity(identityId).Suspend();
void Session.App(appId).Identity(identityId).Reinstate();
```

##### Update an Identity
```
void Session.Identity(identityId).Update(identity => identity.FirstName = "Bob");
```
A Global Admin can update any identity.\
An Org Admin can update any identity from within their organisation.\
No one else can update identities.

##### Resend Verification Email
```
void Session.Identity(identityId).ResendVerificationEmail();
```
A Global Admin can resend a verifaction email to any identity.\
An App Owner can resend a verification email to any identity of apps they own.\
An Org Admin can resend a verification email to any identity from within their organisation.\
No one else can resend verification emails.\
Only unverified identities can be sent a verification email.
