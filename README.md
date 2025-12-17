# 📝 ToDoApp – WPF & MySQL

ToDoApp is a simple **WPF desktop application** that allows users to manage tasks using a secure authentication system and a clean, modern user interface.

---

## 🚀 Features

- Secure login system with hashed passwords
- Create, update, and delete tasks
- Mark tasks as completed
- Optional expense value per task
- Modern WPF user interface
- MySQL database backend

---

## 🧱 Technologies Used

- C# / .NET (WPF)
- MySQL
- MySql.Data
- PBKDF2 password hashing (Rfc2898DeriveBytes)

---

## 📁 Project Structure

```text
ToDoApp
│
├── App.xaml
├── App.xaml.cs
│
├── LoginWindow.xaml
├── LoginWindow.xaml.cs
│
├── MainWindow.xaml
├── MainWindow.xaml.cs
│
├── Security
│   └── PasswordHasher.cs
│
└── README.md
