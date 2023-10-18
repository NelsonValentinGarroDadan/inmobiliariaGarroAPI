-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 18-10-2023 a las 19:27:21
-- Versión del servidor: 10.4.27-MariaDB
-- Versión de PHP: 8.2.0

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmobiliariagarroapi`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `alquileres`
--

CREATE TABLE `alquileres` (
  `Id` int(11) NOT NULL,
  `Precio` decimal(10,0) NOT NULL,
  `Fecha_Inicio` date DEFAULT NULL,
  `Fecha_Fin` date DEFAULT NULL,
  `InquilinoId` int(11) NOT NULL,
  `InmuebleId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `alquileres`
--

INSERT INTO `alquileres` (`Id`, `Precio`, `Fecha_Inicio`, `Fecha_Fin`, `InquilinoId`, `InmuebleId`) VALUES
(4, '12', '2023-09-14', '2023-09-20', 2, 8),
(5, '12', '2023-09-14', '2023-09-20', 2, 8),
(6, '12', '2023-09-14', '2023-09-20', 2, 8),
(7, '12', '2023-09-14', '2023-09-20', 2, 8),
(8, '12', '2023-09-14', '2023-09-20', 2, 8),
(9, '12', '2023-09-14', '2023-09-20', 2, 8),
(10, '12', '2023-09-14', '2023-09-20', 2, 8),
(11, '12', '2023-09-14', '2023-09-20', 2, 8),
(12, '12', '2023-09-14', '2023-09-20', 2, 8),
(13, '12', '2023-09-14', '2023-09-20', 2, 8);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmuebles`
--

CREATE TABLE `inmuebles` (
  `Id` int(11) NOT NULL,
  `Longitud` varchar(200) NOT NULL,
  `Latitud` varchar(200) NOT NULL,
  `CAmbientes` int(11) NOT NULL,
  `Tipo` varchar(200) NOT NULL,
  `Uso` varchar(200) NOT NULL,
  `Precio` decimal(10,0) NOT NULL,
  `Disponible` tinyint(1) NOT NULL,
  `PropietarioId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmuebles`
--

INSERT INTO `inmuebles` (`Id`, `Longitud`, `Latitud`, `CAmbientes`, `Tipo`, `Uso`, `Precio`, `Disponible`, `PropietarioId`) VALUES
(3, '-187', '123', 2, 'Casa', 'Recidencial', '12000', 1, 4),
(4, '-187', '123', 2, 'Casa', 'Recidencial', '12000', 1, 5),
(5, '-187', '123', 2, 'Casa', 'Recidencial', '12000', 1, 5),
(6, '-187', '123', 2, 'Casa', 'Recidencial', '12000', 1, 5),
(7, '-187', '1276', 2, 'Casa', 'Recidencial', '12000', 1, 5),
(8, '-187', '1276', 2, 'Casa', 'Recidencial', '12000', 1, 5),
(9, '-187', '1276', 1, 'Monoambiente', 'Recidencial', '10', 0, 5);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilinos`
--

CREATE TABLE `inquilinos` (
  `Id` int(11) NOT NULL,
  `PersonaId` int(11) NOT NULL,
  `Longitud` varchar(250) NOT NULL,
  `Latitud` varchar(250) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilinos`
--

INSERT INTO `inquilinos` (`Id`, `PersonaId`, `Longitud`, `Latitud`) VALUES
(2, 4, '-180', '123456');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pagos`
--

CREATE TABLE `pagos` (
  `Id` int(11) NOT NULL,
  `NroPago` int(11) NOT NULL,
  `AlquilerId` int(11) NOT NULL,
  `Fecha` date NOT NULL,
  `Importe` decimal(10,0) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pagos`
--

INSERT INTO `pagos` (`Id`, `NroPago`, `AlquilerId`, `Fecha`, `Importe`) VALUES
(1, 1, 13, '2023-09-20', '12');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `personas`
--

CREATE TABLE `personas` (
  `Id` int(11) NOT NULL,
  `DNI` int(11) NOT NULL,
  `Nombre` varchar(50) NOT NULL,
  `Apellido` varchar(50) NOT NULL,
  `Telefono` bigint(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `personas`
--

INSERT INTO `personas` (`Id`, `DNI`, `Nombre`, `Apellido`, `Telefono`) VALUES
(1, 44643099, ' Velentin', ' Garro', 2664900972),
(4, 44643099, 'Alejandra', 'Garro', 2664900972);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietarios`
--

CREATE TABLE `propietarios` (
  `Id` int(11) NOT NULL,
  `PersonaId` int(11) NOT NULL,
  `Mail` varchar(250) NOT NULL,
  `Password` varchar(250) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietarios`
--

INSERT INTO `propietarios` (`Id`, `PersonaId`, `Mail`, `Password`) VALUES
(4, 1, ' \"valen@mail.com\"', 'GAKKw6Co5EiIGNiZC1OfQC6offL+e8CoEs3SX0LIrA=='),
(5, 4, 'valen@mail.com', 'GAKKw6Co5EiIGNiZC1OfQC6offL+e8CoEs3SX0LIrA==');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `alquileres`
--
ALTER TABLE `alquileres`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `InquilinoId` (`InquilinoId`),
  ADD KEY `InmuebleId` (`InmuebleId`);

--
-- Indices de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `PropietarioId` (`PropietarioId`);

--
-- Indices de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `PersonaId` (`PersonaId`);

--
-- Indices de la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `AlquilerId` (`AlquilerId`);

--
-- Indices de la tabla `personas`
--
ALTER TABLE `personas`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `PersonaId` (`PersonaId`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `alquileres`
--
ALTER TABLE `alquileres`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `pagos`
--
ALTER TABLE `pagos`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT de la tabla `personas`
--
ALTER TABLE `personas`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `alquileres`
--
ALTER TABLE `alquileres`
  ADD CONSTRAINT `alquileres_ibfk_1` FOREIGN KEY (`InquilinoId`) REFERENCES `inquilinos` (`Id`),
  ADD CONSTRAINT `alquileres_ibfk_2` FOREIGN KEY (`InmuebleId`) REFERENCES `inmuebles` (`Id`);

--
-- Filtros para la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD CONSTRAINT `inmuebles_ibfk_1` FOREIGN KEY (`PropietarioId`) REFERENCES `propietarios` (`Id`);

--
-- Filtros para la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  ADD CONSTRAINT `inquilinos_ibfk_1` FOREIGN KEY (`PersonaId`) REFERENCES `personas` (`Id`);

--
-- Filtros para la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD CONSTRAINT `pagos_ibfk_1` FOREIGN KEY (`AlquilerId`) REFERENCES `alquileres` (`Id`);

--
-- Filtros para la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD CONSTRAINT `propietarios_ibfk_1` FOREIGN KEY (`PersonaId`) REFERENCES `personas` (`Id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
