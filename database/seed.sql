-- ============================================
-- DATOS DE PRUEBA (hashes BCrypt reales $2a$12$)
-- Passwords: paciente123 (pacientes) / medico123 (médicos)
-- IMPORTANTE: ejecutar después de reset.sql (o sobre BD vacía)
-- ============================================

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