-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
-- -----------------------------------------------------
-- Schema ADOPSE
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema ADOPSE
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `ADOPSE` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci ;
USE `ADOPSE` ;

-- -----------------------------------------------------
-- Table `ADOPSE`.`Category`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `ADOPSE`.`Category` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Name` LONGTEXT NOT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB
AUTO_INCREMENT = 13
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `ADOPSE`.`Difficulty`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `ADOPSE`.`Difficulty` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`Id`, `Name`))
ENGINE = InnoDB
AUTO_INCREMENT = 4
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `ADOPSE`.`Lecturer`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `ADOPSE`.`Lecturer` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Name` LONGTEXT NOT NULL,
  `Bio` LONGTEXT NOT NULL,
  `Website` LONGTEXT NOT NULL,
  `Email` LONGTEXT NOT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB
AUTO_INCREMENT = 10
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `ADOPSE`.`ModuleType`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `ADOPSE`.`ModuleType` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Name` LONGTEXT NOT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB
AUTO_INCREMENT = 4
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `ADOPSE`.`SubCategory`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `ADOPSE`.`SubCategory` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Name` LONGTEXT NOT NULL,
  `parentId` INT NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `IX_SubCategory_parentId` (`parentId` ASC) VISIBLE,
  CONSTRAINT `FK_SubCategory_Category_parentId`
    FOREIGN KEY (`parentId`)
    REFERENCES `ADOPSE`.`Category` (`Id`)
    ON DELETE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 156
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `ADOPSE`.`Module`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `ADOPSE`.`Module` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Name` LONGTEXT NOT NULL,
  `Description` LONGTEXT NOT NULL,
  `Created` DATETIME(6) NOT NULL,
  `Completed` DATETIME(6) NOT NULL,
  `LeaderId` INT NOT NULL,
  `GoogleCalendarID` LONGTEXT NOT NULL,
  `Price` INT NOT NULL,
  `Rating` INT NOT NULL,
  `SubCategoryId` INT NOT NULL,
  `DifficultyId` INT NOT NULL,
  `DifficultyName` VARCHAR(255) NOT NULL,
  `ModuleTypeId` INT NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `IX_Module_DifficultyId_DifficultyName` (`DifficultyId` ASC, `DifficultyName` ASC) VISIBLE,
  INDEX `IX_Module_LeaderId` (`LeaderId` ASC) VISIBLE,
  INDEX `IX_Module_ModuleTypeId` (`ModuleTypeId` ASC) VISIBLE,
  INDEX `IX_Module_SubCategoryId` (`SubCategoryId` ASC) VISIBLE,
  CONSTRAINT `FK_Module_Difficulty_DifficultyId_DifficultyName`
    FOREIGN KEY (`DifficultyId` , `DifficultyName`)
    REFERENCES `ADOPSE`.`Difficulty` (`Id` , `Name`)
    ON DELETE CASCADE,
  CONSTRAINT `FK_Module_Lecturer_LeaderId`
    FOREIGN KEY (`LeaderId`)
    REFERENCES `ADOPSE`.`Lecturer` (`Id`)
    ON DELETE CASCADE,
  CONSTRAINT `FK_Module_ModuleType_ModuleTypeId`
    FOREIGN KEY (`ModuleTypeId`)
    REFERENCES `ADOPSE`.`ModuleType` (`Id`)
    ON DELETE CASCADE,
  CONSTRAINT `FK_Module_SubCategory_SubCategoryId`
    FOREIGN KEY (`SubCategoryId`)
    REFERENCES `ADOPSE`.`SubCategory` (`Id`)
    ON DELETE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 30306
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `ADOPSE`.`Student`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `ADOPSE`.`Student` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Username` LONGTEXT NOT NULL,
  `Password` LONGTEXT NOT NULL,
  `Email` LONGTEXT NOT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `ADOPSE`.`Enrolled`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `ADOPSE`.`Enrolled` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `StudentId` INT NOT NULL,
  `ModuleId` INT NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `IX_Enrolled_ModuleId` (`ModuleId` ASC) VISIBLE,
  INDEX `IX_Enrolled_StudentId` (`StudentId` ASC) VISIBLE,
  CONSTRAINT `FK_Enrolled_Module_ModuleId`
    FOREIGN KEY (`ModuleId`)
    REFERENCES `ADOPSE`.`Module` (`Id`)
    ON DELETE CASCADE,
  CONSTRAINT `FK_Enrolled_Student_StudentId`
    FOREIGN KEY (`StudentId`)
    REFERENCES `ADOPSE`.`Student` (`Id`)
    ON DELETE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `ADOPSE`.`Event`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `ADOPSE`.`Event` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `GoogleCalendarID` LONGTEXT NOT NULL,
  `ModuleId` INT NOT NULL,
  `Name` LONGTEXT NOT NULL,
  `Details` LONGTEXT NOT NULL,
  `Starts` DATETIME(6) NOT NULL,
  `Ends` DATETIME(6) NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `IX_Event_ModuleId` (`ModuleId` ASC) VISIBLE,
  CONSTRAINT `FK_Event_Module_ModuleId`
    FOREIGN KEY (`ModuleId`)
    REFERENCES `ADOPSE`.`Module` (`Id`)
    ON DELETE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `ADOPSE`.`__EFMigrationsHistory`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `ADOPSE`.`__EFMigrationsHistory` (
  `MigrationId` VARCHAR(150) NOT NULL,
  `ProductVersion` VARCHAR(32) NOT NULL,
  PRIMARY KEY (`MigrationId`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `ADOPSE`.`table_name`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `ADOPSE`.`table_name` (
  `id` INT NOT NULL AUTO_INCREMENT COMMENT 'Primary Key',
  `create_time` DATETIME NULL DEFAULT NULL COMMENT 'Create Time',
  `name` VARCHAR(255) NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
