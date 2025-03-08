# Supermarket POS

This repository contains the source code for a Supermarket Point of Sale (POS) system developed as a university group project by a team of 10 people.

## Table of Contents
- [Introduction](#introduction)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Installation](#installation)
- [Usage](#usage)
- [Screenshots](#screenshots)
- [Contributors](#contributors)
- [License](#license)

## Introduction
The Supermarket POS system is designed to streamline the checkout process in a supermarket. It allows cashiers to scan items, manage inventory, process payments, and generate receipts. The system also includes features for managing discounts, promotions, and customer loyalty programs.

## Features
- Easy-to-use interface for cashiers
- Barcode scanning for quick item entry
- Inventory management
- Payment processing (cash, card)
- Receipt generation
- Discount and promotion management
- Customer loyalty program

## Technologies Used
- **Programming Language:** C#
- **Framework:** .NET
- **Database:** SQL Server

## Installation
To install and run the Supermarket POS system locally, follow these steps:

1. **Clone the repository:**
   ```bash
   git clone https://github.com/ravindusj/supermarket-pos.git
   cd supermarket-pos
   ```

2. **Set up the database:**
   - Ensure SQL Server is installed and running.
   - Run the SQL scripts provided in the `db` directory to create the necessary database and tables.

3. **Configure the application:**
   - Open the project in your preferred C# IDE (e.g., Visual Studio).
   - Update the database connection string in the `appsettings.json` file.

4. **Build and run the application:**
   - Build the solution.
   - Run the application.

## Usage
1. **Log in:** Enter your credentials to log in to the system.
2. **Scan items:** Use a barcode scanner or manually enter item codes to add items to the cart.
3. **Process payment:** Select the payment method and process the payment.
4. **Print receipt:** Generate and print the receipt for the customer.
5. **Manage inventory:** Use the inventory management features to add, update, or remove items from the inventory.
6. **Apply discounts:** Use the discount management features to apply discounts and promotions.

## Screenshots
![Dashboard](https://github.com/ravindusj/supermarket-pos/blob/master/dashboard%20billing.png)
![Inventory](https://github.com/ravindusj/supermarket-pos/blob/master/inventory.png)
![Report](https://github.com/ravindusj/supermarket-pos/blob/master/report.png)
![Staff](https://github.com/ravindusj/supermarket-pos/blob/master/staff.png)
![Staff Analysis](https://github.com/ravindusj/supermarket-pos/blob/master/report_inventory.png)

## Contributors
This project was developed by a group of 10 university students. The team members are:
- [Mevan Manilka](https://github.com/mevanmanilka83)
- [Danuja Mayadunne](https://github.com/danujamayadunne)
- [Udula Mayadunne](https://github.com/udula-m)
- [Kanishka Gayanath](https://github.com/kanishkagayanath)
- [Sanka Sathsara](https://github.com/sanka-dev)
- [Chathur Shavinda](https://github.com/chathurshavinda)
- [Pahan Thilanka](https://github.com/rptilanka)
- [Contributor 8](https://github.com/contributor8)
- [Contributor 9](https://github.com/contributor9)
- [Contributor 10](https://github.com/contributor10)

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
