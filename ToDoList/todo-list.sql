-- phpMyAdmin SQL Dump
-- version 5.1.3
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Erstellungszeit: 27. Okt 2024 um 12:48
-- Server-Version: 10.4.24-MariaDB
-- PHP-Version: 8.0.18

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";

--
-- Datenbank: `test`
--

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `category`
--

CREATE TABLE `category` (
  `id` int(11) NOT NULL,
  `description` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Daten für Tabelle `category`
--

INSERT INTO `category` (`id`, `description`) VALUES
(1, 'Projekt'),
(2, 'Büroarbeit');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `task`
--

CREATE TABLE `task` (
  `id` int(11) NOT NULL,
  `userId` int(11) NOT NULL,
  `categoryId` int(11) NOT NULL,
  `priority` tinyint(2) NOT NULL CHECK (1 or 2 or 3),
  `title` varchar(300) NOT NULL,
  `expense` decimal(4,2) DEFAULT NULL,
  `done` tinyint(1) NOT NULL DEFAULT 0,
  `created` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Daten für Tabelle `task`
--

INSERT INTO `task` (`id`, `userId`, `categoryId`, `priority`, `title`, `expense`, `done`, `created`) VALUES
(1, 0, 2, 0, 'E-Mail schreiben', '1.00', 0, '2024-10-15 13:47:11'),
(3, 0, 1, 0, 'Task abhaken realisiersen', '5.00', 1, '2024-10-18 12:02:03'),
(4, 0, 1, 0, 'Task anzeigen', '2.00', 0, '2024-10-18 12:04:58');

--
-- Indizes der exportierten Tabellen
--

--
-- Indizes für die Tabelle `category`
--
ALTER TABLE `category`
  ADD PRIMARY KEY (`id`);

--
-- Indizes für die Tabelle `task`
--
ALTER TABLE `task`
  ADD PRIMARY KEY (`id`),
  ADD KEY `FK_todoCategory` (`categoryId`);

--
-- AUTO_INCREMENT für exportierte Tabellen
--

--
-- AUTO_INCREMENT für Tabelle `category`
--
ALTER TABLE `category`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT für Tabelle `task`
--
ALTER TABLE `task`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- Constraints der exportierten Tabellen
--

--
-- Constraints der Tabelle `task`
--
ALTER TABLE `task`
  ADD CONSTRAINT `FK_todoCategory` FOREIGN KEY (`categoryId`) REFERENCES `category` (`id`);
COMMIT;
