-- ==========================================================
-- MÓDULO A: Pacientes y Médicos
-- Equipo 5 — Modelo de Datos
-- Motor: PostgreSQL (Supabase)
-- Ejecutar completo en: Supabase Dashboard -> SQL Editor
-- ==========================================================

CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- ----------------------------------------------------------
-- Tipos ENUM
-- ----------------------------------------------------------
CREATE TYPE rol_usuario AS ENUM ('paciente', 'medico', 'admin');
CREATE TYPE estado_usuario AS ENUM ('activo', 'inactivo');

-- ----------------------------------------------------------
-- Tabla: usuario
-- ----------------------------------------------------------
CREATE TABLE usuario (
    id_usuario      BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    correo          VARCHAR(100) NOT NULL UNIQUE,
    contrasena_hash VARCHAR(255) NOT NULL,
    rol             rol_usuario NOT NULL,
    fecha_registro  TIMESTAMP NOT NULL DEFAULT now(),
    estado          estado_usuario NOT NULL DEFAULT 'activo'
);

-- ----------------------------------------------------------
-- Tabla: especialidad
-- ----------------------------------------------------------
CREATE TABLE especialidad (
    id_especialidad     INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    nombre_especialidad VARCHAR(60) NOT NULL UNIQUE,
    descripcion         VARCHAR(255)
);

-- ----------------------------------------------------------
-- Tabla: paciente
-- ----------------------------------------------------------
CREATE TABLE paciente (
    id_paciente       BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    id_usuario        BIGINT UNIQUE REFERENCES usuario(id_usuario) ON DELETE SET NULL,
    matricula         VARCHAR(15) NOT NULL UNIQUE,
    nombre            VARCHAR(60) NOT NULL,
    apellido_paterno  VARCHAR(60) NOT NULL,
    apellido_materno  VARCHAR(60),
    fecha_nacimiento  DATE NOT NULL,
    sexo              CHAR(1) NOT NULL,
    tipo_sangre       VARCHAR(3),
    telefono          VARCHAR(15),
    carrera           VARCHAR(80) NOT NULL,
    semestre          SMALLINT NOT NULL,
    CONSTRAINT ck_paciente_semestre CHECK (semestre BETWEEN 1 AND 12),
    CONSTRAINT ck_paciente_sexo CHECK (sexo IN ('M', 'F'))
);

-- ----------------------------------------------------------
-- Tabla: medico
-- ----------------------------------------------------------
CREATE TABLE medico (
    id_medico           BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    id_usuario          BIGINT UNIQUE REFERENCES usuario(id_usuario) ON DELETE SET NULL,
    id_especialidad     INT NOT NULL REFERENCES especialidad(id_especialidad) ON DELETE RESTRICT,
    cedula_profesional  VARCHAR(20) NOT NULL UNIQUE,
    nombre              VARCHAR(60) NOT NULL,
    apellido_paterno    VARCHAR(60) NOT NULL,
    apellido_materno    VARCHAR(60),
    telefono            VARCHAR(15) NOT NULL
);

-- ----------------------------------------------------------
-- Índices adicionales para búsquedas frecuentes
-- ----------------------------------------------------------
CREATE INDEX idx_paciente_matricula ON paciente(matricula);
CREATE INDEX idx_medico_especialidad ON medico(id_especialidad);

-- ==========================================================
-- DATOS DE PRUEBA (seed)
-- ----------------------------------------------------------
-- Passwords de prueba: paciente123 (pacientes) / medico123 (médicos)
-- Hashes BCrypt $2a$12$ generados con BCrypt.Net (verificables por la API)
-- IMPORTANTE: ejecutar sobre una BD vacía o después de reset.sql.
-- ==========================================================

-- Especialidades
INSERT INTO especialidad (nombre_especialidad, descripcion) VALUES
('Medicina General', 'Atención médica primaria y general'),
('Odontología', 'Salud dental y bucal'),
('Psicología', 'Salud mental y bienestar emocional'),
('Nutrición', 'Asesoría alimenticia y nutricional');

-- Usuarios (2 pacientes con cuenta, 2 médicos)
INSERT INTO usuario (correo, contrasena_hash, rol, estado) VALUES
('jorge.salazar@uadeo.edu.mx', '$2a$12$zDywWz/dZcYcOOyx6VBF8.y3LoniLu8UICMhgZ6IRzFKtti2Hs6ES', 'paciente', 'activo'),
('sonia.meza@uadeo.edu.mx', '$2a$12$8pgNVIVbgic5PaF5RyO.pOa0M4w.TaiYmsRLbv85WDvBfq9PimPUu', 'paciente', 'activo'),
('dr.lopez@uadeo.edu.mx', '$2a$12$26pVomd0Yx0w1d6uajxSEOD8cGbkL/v/96ya82w75FwpzjdcMkrHa', 'medico', 'activo'),
('dra.chaidez@uadeo.edu.mx', '$2a$12$Cfv7FkprACZddC5AnLhEa.jtHDiYykzNUH0YNoKD6nVDq3bvqr3h2', 'medico', 'activo');

-- Pacientes (1 sin cuenta, id_usuario NULL)
INSERT INTO paciente (id_usuario, matricula, nombre, apellido_paterno, apellido_materno, fecha_nacimiento, sexo, tipo_sangre, telefono, carrera, semestre) VALUES
(1, '24040040', 'Jorge Silvano', 'Salazar', 'Vega', '2004-03-15', 'M', 'O+', '6671234567', 'Ingeniería en Software', 6),
(2, '24040356', 'Sonia Yasmin', 'Meza', 'Pérez', '2003-11-02', 'F', 'A+', '6677654321', 'Ingeniería en Software', 6),
(NULL, '23038211', 'Angie Guadalupe', 'Rios', 'Zazueta', '2002-07-20', 'F', NULL, NULL, 'Ingeniería Industrial', 8);

-- Médicos
INSERT INTO medico (id_usuario, id_especialidad, cedula_profesional, nombre, apellido_paterno, apellido_materno, telefono) VALUES
(3, 1, 'CP1234567', 'Sergio Octavio', 'López', 'Sainz', '6679876543'),
(4, 3, 'CP7654321', 'Andrew Neftalí', 'Chaidez', 'López', '6671122334');