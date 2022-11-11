-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Nov 10, 2022 at 04:46 PM
-- Server version: 10.4.25-MariaDB
-- PHP Version: 8.1.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `grocery_shopping`
--
CREATE DATABASE IF NOT EXISTS `grocery_shopping` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `grocery_shopping`;

-- --------------------------------------------------------

--
-- Table structure for table `consumers`
--

DROP TABLE IF EXISTS `consumers`;
CREATE TABLE IF NOT EXISTS `consumers` (
  `consumerid` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  `gender` varchar(50) NOT NULL,
  `email` varchar(50) NOT NULL,
  `mobile` varchar(50) NOT NULL,
  `password` varchar(50) NOT NULL,
  PRIMARY KEY (`consumerid`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `consumers`
--

INSERT INTO `consumers` (`consumerid`, `name`, `gender`, `email`, `mobile`, `password`) VALUES
(4, 'Adam Copeland', 'Male', 'adam.c@gmail.com', '8546917313', '654321');

-- --------------------------------------------------------

--
-- Table structure for table `grocery`
--

DROP TABLE IF EXISTS `grocery`;
CREATE TABLE IF NOT EXISTS `grocery` (
  `groceryname` varchar(50) NOT NULL,
  `price_per_unit` decimal(10,2) NOT NULL,
  PRIMARY KEY (`groceryname`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `grocery`
--

INSERT INTO `grocery` (`groceryname`, `price_per_unit`) VALUES
('Apple', '8.00'),
('Banana', '4.00'),
('Beans - Haricot', '3.00'),
('Capsicum - Red', '9.00'),
('Colgate', '11.00'),
('Cothas coffee', '10.00'),
('Mosambi', '7.00'),
('Onion', '1.00'),
('Orange', '5.00'),
('Potato', '2.00'),
('Pure Ghee Tuppa', '12.00'),
('Surf Excel', '13.00'),
('Sweet Corn', '6.00'),
('Tata Salt', '15.00'),
('Tata Tea Gold', '14.00');

-- --------------------------------------------------------

--
-- Table structure for table `orders`
--

DROP TABLE IF EXISTS `orders`;
CREATE TABLE IF NOT EXISTS `orders` (
  `ordernumber` int(11) NOT NULL AUTO_INCREMENT,
  `orderdate` date DEFAULT curdate(),
  `consumerid` int(11) NOT NULL,
  `billamount` decimal(10,2) DEFAULT 0.00,
  `groceryname` varchar(50) NOT NULL,
  `billstatus` varchar(50) DEFAULT 'NOT PAID',
  PRIMARY KEY (`ordernumber`),
  KEY `fk1` (`consumerid`),
  KEY `fk2` (`groceryname`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `orders`
--

INSERT INTO `orders` (`ordernumber`, `orderdate`, `consumerid`, `billamount`, `groceryname`, `billstatus`) VALUES
(16, '2022-11-10', 4, '82.00', 'Onion', 'PAID');

--
-- Constraints for dumped tables
--

--
-- Constraints for table `orders`
--
ALTER TABLE `orders`
  ADD CONSTRAINT `fk1` FOREIGN KEY (`consumerid`) REFERENCES `consumers` (`consumerid`),
  ADD CONSTRAINT `fk2` FOREIGN KEY (`groceryname`) REFERENCES `grocery` (`groceryname`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
