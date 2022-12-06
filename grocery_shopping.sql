CREATE DATABASE IF NOT EXISTS grocery_shopping;
USE grocery_shopping;

DROP TABLE IF EXISTS consumers;
CREATE TABLE IF NOT EXISTS consumers (
  consumerid int(11) NOT NULL AUTO_INCREMENT,
  name varchar(50) NOT NULL,
  gender varchar(50) NOT NULL,
  email varchar(50) NOT NULL,
  mobile varchar(50) NOT NULL,
  password varchar(50) NOT NULL,
  Role varchar(50) DEFAULT NULL,
  PRIMARY KEY (consumerid)
);


DROP TABLE IF EXISTS grocery;
CREATE TABLE IF NOT EXISTS grocery (
  groceryname varchar(50) NOT NULL,
  price_per_unit decimal(10,2) NOT NULL,
  PRIMARY KEY (groceryname)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

INSERT INTO grocery (groceryname, price_per_unit) VALUES
('Apple', '3.00'),
('Banana', '1.30'),
('Beans - Haricot', '1.20'),
('Capsicum - Red', '1.30'),
('Colgate', '2.50'),
('Cothas Coffee', '2.00'),
('Mosambi', '1.00'),
('Onion', '1.00'),
('Orange', '2.00'),
('Potato', '1.15'),
('Pure Ghee Tuppa', '5.00'),
('Surf Excel', '2.00'),
('Sweet Corn', '2.00'),
('Tata Salt', '0.50'),
('Tata Tea Gold', '1.10');


DROP TABLE IF EXISTS orders;
CREATE TABLE IF NOT EXISTS orders (
  ordernumber int(11) NOT NULL AUTO_INCREMENT,
  orderdate date DEFAULT curdate(),
  consumerid int(11) NOT NULL,
  billamount decimal(10,2) DEFAULT 0.00,
  groceryname varchar(50) NOT NULL,
  billstatus varchar(50) DEFAULT 'NOT PAID',
  PayMode varchar(100) DEFAULT NULL,
  PayAmount decimal(10,2) DEFAULT NULL,
  DueAmount decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (ordernumber),
  KEY fk1 (consumerid),
  KEY fk2 (groceryname)
);


ALTER TABLE orders
  ADD CONSTRAINT fk1 FOREIGN KEY (consumerid) REFERENCES consumers (consumerid),
  ADD CONSTRAINT fk2 FOREIGN KEY (groceryname) REFERENCES grocery (groceryname);
COMMIT;