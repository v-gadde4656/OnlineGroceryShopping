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
  PRIMARY KEY (consumerid)
);

DROP TABLE IF EXISTS grocery;
CREATE TABLE IF NOT EXISTS grocery (
  groceryname varchar(50) NOT NULL,
  price_per_unit decimal(10,2) NOT NULL,
  PRIMARY KEY (groceryname)
);

DROP TABLE IF EXISTS orders;
CREATE TABLE IF NOT EXISTS orders (
  ordernumber int(11) NOT NULL AUTO_INCREMENT,
  orderdate date DEFAULT curdate(),
  consumerid int(11) NOT NULL,
  billamount decimal(10,2) DEFAULT 0.00,
  groceryname varchar(50) NOT NULL,
  billstatus varchar(50) DEFAULT 'NOT PAID',
  PRIMARY KEY (ordernumber),
  KEY fk1 (consumerid),
  KEY fk2 (groceryname)
);

ALTER TABLE orders
  ADD CONSTRAINT fk1 FOREIGN KEY (consumerid) REFERENCES consumers (consumerid),
  ADD CONSTRAINT fk2 FOREIGN KEY (groceryname) REFERENCES grocery (groceryname);
COMMIT;
