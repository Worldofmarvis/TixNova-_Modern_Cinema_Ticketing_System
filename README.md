<p align="center">

  <img width="516" height="121" alt="UML" src="https://github.com/user-attachments/assets/c462edf2-ce07-42db-b444-cdc1d1f186c3" />

</p>
<h1 align="center">TixNova+ | Modern Cinema Ticketing System</h1>

##  Project Description

TixNova+ is a modern, feature rich desktop application built using **C# and .NET Windows Forms**, designed to simulate a complete cinema ticketing experience. Moving away from the traditional, rigid look of standard WinForms, TixNova+ prioritizes user experience with a highly customized, visually appealing interface. The application utilizes custom-built UI controls including rounded gradient buttons, glowing labels, and native Windows glass/acrylic effects. With an intuitive dashboard, users can effortlessly browse movies by genre or age rating, navigate custom search menus, check cinema availability, and seamlessly book tickets within a fluid, dynamic desktop environment.

---

##  UML Diagram
<p align="center">
  <img width="1581" height="995" alt="UML" src="https://github.com/user-attachments/assets/1f18beb9-9333-4fa9-a2a9-5d0fc9ec5367" />
</p>

---

##  Features

### Main Dashboard
The Main Dashboard serves as the **central hub** of the application, highlighting today's **trending movies** while providing seamless navigation to various modules. The Movies section displays both **Now Showing** and **Coming Soon** titles. Users can explore different locations and view their ratings in the **Cinemas module**, or pre-order snacks through the built-in **Shop**. For targeted browsing, the Categories section efficiently organizes films by **genre and age ratings**. Additionally, a robust **Search Menu** ensures quick and easy navigation throughout the entire system.

### Advanced Ticketing & Booking
TixNova+ takes the booking experience further with an **interactive visual seat selection** system that allows users to easily pick their preferred spots. The system includes **secure double-booking prevention** to ensure a flawless reservation process. Upon checkout completion, the application generates a **comprehensive digital ticket**, containing all necessary booking details for a seamless movie-going experience.

### Integrated Concession Shop
To complete the cinematic experience, TixNova+ features a **dedicated shop module** that allows users to seamlessly **pre-order snacks, and beverages** alongside their movie tickets. The shop is powered by a **simulated dynamic inventory** system, ensuring that product availability is updated and accurately displayed as users add items to their purchases.

### Account Management
To personalize the user experience, the system includes robust **user session handling** and **secure login authentication**. Once logged in, users have access to their own accounts, allowing for **personalized booking history tracking** so they can easily review their past movie choices and transactions.

### Custom UI Design
Built from the ground up to break away from traditional WinForms, TixNova+ boasts a **highly customized and modern aesthetic**. The application features **real-time search filtering** and utilizes **native Windows glass and acrylic effects**. Designed to be **visually appealing and intuitive**, the system incorporates custom C# drawing classes to create dynamic elements like **glowing labels, rounded gradient buttons, and fading image buttons** to maximize user comfort and engagement.

---

## Object-Oriented Programming (OOP) Principles

TixNova+ is architected using core Object-Oriented Programming principles to ensure the codebase remains scalable, maintainable, and highly modular:

* **Encapsulation:** Sensitive data, such as user session details, booking states, and shop inventory counts, are securely hidden within their respective classes. Access to these properties is strictly controlled through public methods and getter/setter properties, preventing unintended external modifications and ensuring data integrity.
* **Inheritance:** The application extensively leverages inheritance to create its highly customized UI. Custom controls, such as the rounded gradient buttons, glowing link labels, and custom drop-down menus, inherit directly from base Windows Forms classes. This allows the app to extend default behaviors without rewriting fundamental WinForms logic.
* **Polymorphism:** Through method overriding, TixNova+ implements dynamic UI behaviors. Standard rendering methods are overridden in custom controls to draw smooth gradients, custom rounded paths, and glowing effects instead of relying on the default rigid WinForms graphics.
* **Abstraction:** Complex backend logic such as seat conflict resolution, ticket generation, and custom blur-behind API calls is abstracted away from the front-end interface. The UI forms simply interact with high-level methods, hiding the underlying complexity of state management and memory handling.

---

## Quick Overview of the System

### Login Form
<img width="1919" height="1079" alt="Login Page" src="https://github.com/user-attachments/assets/dc05cc2c-85d3-44fe-9822-792b5d938b5a" />

### User Main DashBoard
<img width="1919" height="1003" alt="User Main DashBoard" src="https://github.com/user-attachments/assets/1a05c8c7-292d-43eb-8caf-1f3292162936" />

---

##  How It Works

TixNova+ is designed to provide a seamless, end-to-end user journey, simulating a real-world cinema kiosk or desktop booking software:

1. **User Authentication:** The application starts at the secure **Login Form**. Users must authenticate to establish a secure session, enabling personalized tracking of their booking history.
2. **Exploration & Discovery:** Upon logging in, users are directed to the **Main Dashboard**. From here, they can dynamically filter movies by genre (Action, Sci-Fi, Horror, etc.) or age rating (G, PG-13, SPG), or use the custom search bar to find specific titles.
3. **Cinema & Showtime Selection:** Users can browse different cinema branches via the **Cinemas module** to check ratings, locations, and available showtimes for their desired movie.
4. **Interactive Seat Selection:** Once a movie and schedule are selected, the system opens an interactive visual seat map. Users click to reserve their desired seats, while the system runs background checks to ensure those seats aren't already taken, preventing double-booking conflicts.
5. **Concessions (Optional):** Before final checkout, users can navigate to the **Shop module** to add popcorn, drinks, and other snacks to their transaction. The simulated inventory updates in real-time.
6. **Ticket Generation & Checkout:** After finalizing their seats and snacks, the user proceeds to checkout. The system processes the order and generates a digital summary/ticket containing all essential booking details.

---

##  How to Run

To run TixNova+ on your local machine, follow these instructions:

### Prerequisites
* **Visual Studio** (2019 or 2022 recommended)
* **.NET Desktop Development Workload** installed in Visual Studio (required for Windows Forms applications)
* **Git** (optional, for cloning the repository)

### Installation & Execution Steps
1. **Clone the Repository:** Open your terminal or command prompt and run the following command to clone the project to your local machine:
   ```bash
   git clone [https://github.com/JosefuMaeda/TixNova-Final.git](https://github.com/JosefuMaeda/TixNova-Final.git)
   ```
   *(Alternatively, you can download the project as a ZIP file and extract it.)*
2. **Open the Project:** Navigate to the cloned folder and open the `.slnx` or `.csproj` file using Visual Studio.
3. **Build the Solution:** Go to the top menu and select **Build > Build Solution** (or press `Ctrl+Shift+B`). This step is crucial as it compiles the custom UI classes so they render correctly in the designer and during runtime.
4. **Run the Application:** Click the **Start** button at the top of Visual Studio or press `F5` to compile and launch TixNova+.
## Contributors

| Avatar | Name | Role | Github |
| :---: | :--- | :--- | :--- |
| <img src="https://github.com/Worldofmarvis.png" width="40" height="40" style="border-radius: 50%;"> | Edmar D. Visto | Developer | [https://github.com/Worldofmarvis](https://github.com/Worldofmarvis) |
| <img src="https://github.com/jeilyannnmerhan.png" width="40" height="40" style="border-radius: 50%;"> | jeilylyly | Developer | [https://github.com/jeilyannnmerhan](https://github.com/jeilyannnmerhan) |
| <img src="https://github.com/Faijeyy.png" width="40" height="40" style="border-radius: 50%;"> | Aj | Developer | [https://github.com/Faijeyy](https://github.com/Faijeyy) |

```
