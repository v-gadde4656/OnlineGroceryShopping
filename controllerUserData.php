<?php
session_start();
require "connection.php";
$email = "";
$name = "";
$errors = [];

//if user signup button
if (isset($_POST["signup"])) {
    $name = mysqli_real_escape_string($con, $_POST["name"]);
    $email = mysqli_real_escape_string($con, $_POST["email"]);
    $mobile = mysqli_real_escape_string($con, $_POST["mobile"]);
    $gender = mysqli_real_escape_string($con, $_POST["gender"]);
    $password = mysqli_real_escape_string($con, $_POST["password"]);
    $cpassword = mysqli_real_escape_string($con, $_POST["cpassword"]);
    if ($password !== $cpassword) {
        $errors["password"] = "Confirm Password Not Matched!";
    }
    $email_check = "SELECT * FROM consumers WHERE email = '$email'";
    $res = mysqli_query($con, $email_check);
    if (mysqli_num_rows($res) > 0) {
        $errors["email"] = "This Email Already Exists!";
    }
    if (count($errors) === 0) {
        $insert_data = "INSERT INTO consumers (name, gender, email, mobile, password)
                        values('$name', '$gender', '$email', '$mobile', '$password')";
        $data_check = mysqli_query($con, $insert_data);
        if ($data_check) {
            echo "Consumer Signup Successful!";
            header("location: login-consumer.php");
        } else {
            $errors["db-error"] = "Failed while inserting data into database!";
        }
    }
}

//if user click login button
if (isset($_POST["login"])) {
    $email = mysqli_real_escape_string($con, $_POST["email"]);
    $password = mysqli_real_escape_string($con, $_POST["password"]);
    $check_email = "SELECT * FROM consumers WHERE email = '$email'";
    $res = mysqli_query($con, $check_email);
    if (mysqli_num_rows($res) > 0) {
        $fetch = mysqli_fetch_assoc($res);
        $fetch_pass = $fetch["password"];
        if ($password == $fetch_pass) {
            $_SESSION["email"] = $email;
            $_SESSION["password"] = $password;
            header("location: home.php");
        } else {
            $errors["email"] = "Incorrect Email or Password!";
        }
    } else {
        $errors["email"] = "You Need To Signup first";
    }
}

//if login now button click
if (isset($_POST["login-now"])) {
    header("Location: login-consumer.php");
}
?>
