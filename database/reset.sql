-- ============================================
-- RESET: borra todos los datos y reinicia los IDs
-- Ejecutar siempre ANTES de seed.sql
-- ============================================
TRUNCATE TABLE medico, paciente, especialidad, usuario RESTART IDENTITY CASCADE;