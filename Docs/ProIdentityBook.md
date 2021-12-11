# Pro ASP.NET Core Identity

Under the Hood with Authentication and Authorization in ASP.NET Core 5 and 6 Applications

# Part I - Using ASP.NET Core Identity

## 1 - Getting Ready

- ASP.NET Core Identity is the user management system for ASP.NET Core applications.
- It provides an API for managing users and roles and for signing users into and out of applications.
- Users can sign in with simple passwords, use two-factor authentication, or sign in using third-party platforms provided by Google,
Facebook, and Twitter.

## 2 - Your First Identity Application

A simple Identity project.

## 3 - Creating the Example Project

Other simple project, with Authentication and Authorization.

## 4 - Using the Identity UI Package

- The Identity UI package provides all the workflows required for basic user management, including creating accounts and signing in with passwords, authenticators, and third-party services.

## 5 - Configuring Identity

- IdentityOptions:
    - Specify policies for usernames, email addresses, passwords, account confirmations, and lockouts.

- Support third-party services:
    - Google, Facebook, and Twitter.

- Configuring External Authentication:
    - External authentication delegate the process of authenticating users to a third-party service.

## 6 - Adapting Identity UI

- Scaffold is scary.

## 7 - Using the Identity API

- The Identity API provides access to all of the Identity features.
- It can be used to create completely custom workflows.
- Key classes are provided as services that are available through the standard ASP.NET Core dependency injection feature.

- UserManager:
    - Usado para realizar diversas operações relacionadas ao usuário.

## 8 - Signing In and Out and Managing Passwords

- These API features are used to create workflows for signing the user into the application with a password and signing them out again when they have finished their session.

- These features are also used to manage passwords, both to set passwords administratively and to perform self-service password changes and password recovery.

- Passwords are not the only way to authenticate with an Identity application, but they are the most widely used and are required by most projects.

- Passwords are managed using methods provided by the UserManager<User> class, which allows passwords to be added and removed from a user account.

- Users sign into and out of the application using methods defined by the sign-in manager class, SignInManager<T>.

- The sign-in process can be complex, especially if the project supports two-factor authentication and external authentication.

- Managing Passwords:
    - HasPasswordAsync(user)
    - AddPasswordAsync(user, password)
    - RemovePasswordAsync(user)

- Signing In, Signing Out, and Denying Access:
    - Signing into the Application:
        - When signing into the application, the user provides credentials that are compared to the data in the user store.
        - If the credentials match the stored data, then a **cookie** is added to the response that securely identifies the user in subsequent requests.
        - ASP.NET Core uses the **ClaimsPrincipal** class to represent the signed-in user.
        - Evaluating the user’s credentials and creating the ClaimsPrincipal object are the responsibilities of the sign-in manager class, SignInManager<User>, which is configured as a service when Identity is configured.
        - 





# Part II - Understanding ASP.NET Core Identity



