# E-Commerce Web Application

[Deployed website link](https://e-commerce-app.azurewebsites.net)

## Overview
This project is an e-commerce web application created with C# ASP.NET Core MVC. It's designed as an educational project
to demonstrate how to work with ASP. NET MVC.

For this project we chose to go for a tech store that sells all manner of items needed for people interested in all
manner of technical products from laptops to their needed accessories to screens

# Installation Instructions

1. **Clone the Repository**:

   Open your preferred command-line interface and run the following command to clone the project:

git clone https://dev.azure.com/23038513/_git/E-Commerce-App

2. **Open in Visual Studio**:

Launch Visual Studio and open the project solution file (`ECommerceApp.sln`).

3. **Update Database**:

Open the Package Manager Console in Visual Studio by navigating to `Tools > NuGet Package Manager > Package Manager Console`.

Run the following command to apply the database migrations and create the necessary tables:
 ***Update-Database***

 
This will set up the database with the required schema.

4. **Run the Application**:

Click the "Start" button in Visual Studio or press `F5` to run the application. This will launch the development server and open the application in your default web browser.

You should now be able to browse and interact with the application locally.

Please note that if you encounter any issues during the installation process, feel free to reach out to us for assistance.

# Getting Started

To get started with our application, you'll need to sign up. You have the option to sign up as an administrator or an editor.

- As an **administrator**, you'll have access to all the features in our application, including the ability to create new products and categories.

- As an **editor**, you'll be able to edit existing categories.

Once you've signed up and logged in, you'll have access to the following features:

- **Viewing Products and Categories**: You'll be able to browse through all the products and categories available in our application.

- **Shopping Cart**: Add products to your shopping cart for easy purchase.

- **Checkout**: Complete the checkout process to finalize your order.

If you're using the application as an anonymous user, you can still browse and view all products and categories. However, to make purchases, you'll need to sign up.

Feel free to explore our application and start shopping for your favorite tech products!


## Database schema

![Database schema](https://cdn.discordapp.com/attachments/1095054312129966161/1152665664243376138/TeckPioneers.png)

## schema explanation

We chose to work with a one-to-many relation between the categories and the products as we saw it fit that one category
would be sufficient to identify all the products that are within this category and that its difficult to label the
same product under two categories in the tech industry.

How are categories and products linked in the code you might ask?

They are linked where each product has an object of type category. And where each category has a list of objects of type product.

## Features

- **Home Page**: The landing page of the application.
- **Categories**: Users can browse products by category.
- **Products**: Users can view individual product details.
- **User Authentication**: The application includes login and signup services.

- **Order Page**: A dedicated page where users can review and finalize their orders before making a purchase. Users can view a summary of their selected items, adjust quantities, and proceed to checkout.

- **Cart Page**: This page allows users to manage their shopping cart. They can view all the items they have added, adjust quantities, and remove items if needed. The cart page provides a clear overview of the products chosen for purchase.

### User Roles

The application has three user roles:

1. **Administrator**: Can create new products and categories.
2. **Editor**: Can edit existing categories.
3. **User**: Can view products and categories and their details.

### Claims

- **What**: User’s ID, username, and role (admin, editor, or user).
- **Where**: These are captured during the login process.
- **Why**: To personalize the user’s experience and authorize their actions (like creating new products or editing existing categories).

### Policies

- **What**: We're enforcing a role-based policy (Administrator/Editor/User) that restricts access to the (Create/ update/ delete) actions throughout our web application.
- **Why**: To ensure that only users in the “Administrator” role can create or delete new categories and products, and only users in the "Editor" role can edit categories and products.

### Usage

Since this is a web application, it doesn't need to be installed. To use the application, navigate to the deployed URL in your web browser.

### Screenshots
![HomePage](https://lab29ecommerceimages.blob.core.windows.net/projectimages/HomePage.png)
![CategoiesPage](https://lab29ecommerceimages.blob.core.windows.net/projectimages/CategoiesPage.png)
![productsPage](https://lab29ecommerceimages.blob.core.windows.net/projectimages/productsPage.png)
![LoginPage](https://lab29ecommerceimages.blob.core.windows.net/projectimages/LoginPage.png)
![SideBar](https://lab29ecommerceimages.blob.core.windows.net/projectimages/SideBar.png)
![AdminViewProducts](https://lab29ecommerceimages.blob.core.windows.net/projectimages/AdminViewProducts.png)
![AdminDashBoard](https://lab29ecommerceimages.blob.core.windows.net/projectimages/AdminDashBoard.png)
![PurchaseSummary](https://lab29ecommerceimages.blob.core.windows.net/projectimages/PurchaseSummary.png)
![OrderSummeryConfirmation](https://lab29ecommerceimages.blob.core.windows.net/projectimages/OrderSummeryConfirmation.png)
![PaymentCheckout](https://lab29ecommerceimages.blob.core.windows.net/projectimages/PaymentCheckout.png)

### Contributing

- [Ahmad Harhsheh](https://www.linkedin.com/in/ahmad-harhsheh-aa1940231/)
- [AlHareth alhyari](https://www.linkedin.com/in/hareth-alhyari-70b2b3123/)

This being an educational project, contributions are welcome. Please feel free to fork the project and submit your pull requests.


