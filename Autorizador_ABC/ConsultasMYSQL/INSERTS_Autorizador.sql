USE AUTORIZADOR_BANCOABC;

INSERT INTO TIPOS_TARJETAS_TB (
	TIPO_TARJ_Nombre
) VALUES 
('Debito'), 
('Credito');

INSERT INTO TARJETAS_TB (
	TARJETA_Numero,
	TARJETA_PIN,
	TARJETA_FechaVencimiento,
	TARJETA_NumVerificacion,
	TARJETA_TIPO_TARJ_ID
)
VALUES 
('q+KvAFSzNxAJXnFWihhjk8eaIuIDwTiMs5dwfZ0E5E0=', '9y1Ry4ScCb8pINaXGcAG6Q==', 'CqW7AdONktn+xWkMl4dK6A==', 'GI/BxeCs2v3Edz/BTuWENA==', 1),
('uwFStt3StcJlnft1mGvl3qbc7NhVw7MXqzbvAxZ7+Q0=', 'wy5v5KB2Q1HEc5bj5PXimw==', '+nS5iRG7/sji38CDRrbZ6w==', 'AuvneuKbBs7gw5heh/PSHQ==', 1),
('I2uW19bc7Hhs09MREaGIrde7nOvvo9qh0jJq56w0Rqc=', 'aCCEu+NJmE1t5IZjM9DtUA==', 'f99SH65QOdLxMmoZrZy4XQ==', 'cI2RrT26z/cSZe6ShLlf2Q==', 2),
('j6eHifbOV7/9Bz+k4wbVIgJxjLmv0bJDDB5R/BPPuJc=', 'RIoSJxEh5XKGnZQOAQjN3w==', 'odKKNZazvyPAQkFnSJHqqg==', 'aCf6naZWcbyqcqIYKCmSLg==', 2),
('KDBClALT5nD15MBLWdsbKZTUJFToWxDgqzFvQw+9VBI=', 'xaT04MPIOSCA7iaV9a8KZw==', 'DdO9uMC///OAghTeL+uRcg==', '4jaSf/a+N7ZJK9hwrzEOlw==', 1),
('j2nWF9S2UVKkaATt8ySkFl78MaW4MeNK1S629iczGLo=', 'vjtQUERq6oKDnZiZtdwi8w==', 'IjCxO0iuxwFPhdbZ+DjLdA==', 'ENVSmLIrwGTEl5j+cd2AyQ==', 1),
('xN+A0u1FQP+5SPQg9u720VZEvBjVCR3x5wij6iH1Yjo=', 'Orejn5VuLX91OZBCuniWjw==', 'aO6+eTXJIy2lo+jeAtD8pA==', 'JmZY2hCGMXbTXdd670ZcIA==', 2),
('RB+9ALk45qMpiyk1vZsYycmDl0s/K1xpqOaJlEzjJTw=', 'lHjHnMiWJQq1GdgdZkwmHQ==', 'olY53tXzalbD6ysCSX/Ylg==', 'dIEcqkQNpk7uIXDLxWHjYw==', 2),
('YBkG9aLacDonuIGgYYtgmDcTegBJFZywfpJRG+8a3Kc=', 'mwmVHcAp2rhypfEfNjtjGQ==', '6HSNnvNmMuT2cR6BkdyoWQ==', 'JidUzo7KBiPqZC9xQ/On8Q==', 1),
('f5fGV4SpFaLerePFJDYIJG4kZzDVT6C20EwlxeIKllI=', 'mXpZqmfl10O92pH/4fok0w==', '/1B42/zlOAzVjiMeeyLqMQ==', 'kGMKIsW8FHXD5lnQPCvAgQ==', 2),
('ZoKFt+ZKs208m7CDAXZruTlMXJ74Qec2H6zgImUvlRY=', '1xi6rTv7pzAhOWWKX+Cr1w==', 'IkCvo74ALgpfYyYqkxft8w==', 'PbGD/MqzrnCEe09fWLrFmQ==', 1),
('bQWPyhtRM+/NZeheM4VJB8amrWLF5D+JFzncZqhZ8Z8=', '+2QO98ussYgRxOv9RNLFAg==', '4mZLxDXFr/UdSg9JYyDkfQ==', 'rDYl8uPRrsjPSCLwdlxbTg==', 1),
('mmtBdjTZ3fQrtAPcrbYgxqlKlA96jWtYG2JrhjGFYqM=', 'czA7lY9bI6XcbkLU238htg==', '9qX+Kzgjf+s+J0UpMsvlFw==', 'wCFtPG3c6R3SwMHhYv+9RA==', 2),
('CkV6DAY0UXgBuoNqzJU4gRidVF8rUhp+Z9oE2u7NgCo=', 'k3L8xPbnHBIWCPMb3JmRfw==', 'otAB8EHdJAX8q+mzk/rucw==', 'Qn0SIdJ5g7TngUNAU5kECw==', 2),
('hZ8+ZY/Vrr2/RF6vF2J2A7vzNbXJf9tysVJxEr+ZFJw=', 'i4hYZy9d+LLRyWvWUePlMA==', '+dxWAT5CcQTDxOjCZ4J9jg==', 'evGeBsrmyCNhyLzj4hUvaw==', 1),
('LDomkC2mLaQ2lVgJrDtd2zP/U0VRTzzRnrqSKEhnDuE=', 'J6E/ISK6EDDnm0ge+dErdw==', '3B23m7PlvHm0d+HnWuMp8w==', 'aPmiHo6V2q5yd6mpDjy+Eg==', 1),
('MVDavWXGT5XTWc/MJPAmgH7W80XkruFJR0Fai5Y8X6w=', 'OFT3DV1CJwaXSkE3WHCO2g==', 'Pt60l0rqt3r8v4iKZVqqAQ==', 'MpCDGXcuAYcBieipb+7/kw==', 2),
('AQXyim09Vn8yrvGn2aya+Uxcyx3HaoHDRyq/1NKBz6I=', 'sq3+pIezUT0pQTCY3ti+nw==', 'Sz2FAzz4V9zWqcqlUy1hkA==', '7wGDJxbx4sdKXDNizBUzjg==', 2),
('Y7gXpKf/vti9FJnXzBKJCU/GacdUALTkKo+c/eav3HI=', 'v46ioepuzboZdVIm41ieGw==', 'LY/HmB+ZJH6ulwgJLXCpXg==', 'zaK0Mc+dLtI+8UyDxG1cwg==', 1),
('GUhMOSkjmCiB4+ZHq8q7OwudgMwKkfdUlC580sPfAF8=', 'yF1Lck0OwjIdEEummRIFjA==', 'olY53tXzalbD6ysCSX/Ylg==', 'eCT4szqHJNozR3XQmbk8zg==', 2),
('v8Uksl2TRI6ID+CJSQEZXYprkcj8HKGc/675wSrQQdQ=', 'HIDSUGPPG30JXX35sn9Lsg==', 'GzHI3e6MSQ8oEHctuzl1JA==', 'rmimeJ4BLwnV9fOaxpYhtA==', 1),
('ml6hbnAPmtdHCYDcgPl3IWEjLRnOqX2GByuFDBpUh78=', '4dNQ564bcwTyBvkZudDMhg==', 'vyKfHsQ8VHQv8PEE/KSGjg==', 'QDA7PO8YyVbLSvazgJRUYQ==', 2),
('i0exH1QnBtA95miEe5F5BxpciU1+g9FnS2DLnInyKKs=', '1qdDR8SMhc1Bfa80/QKfjg==', 'x2A6vIvg6Cd/kUQ9pdO0UQ==', 'SBe/scJcbKegGnvCLfyY1A==', 1),
('/vWIBzUR/hPZMh+kEXoFutC6av7TNSX26FeYJ7YKiqY=', 'uBcwbEIUDoPr7a747rRbmA==', '8OQjBMKKiex7/hZgIvXdTg==', 'yIB8PauHUHYJzHqqci815w==', 2),
('hiAaIVCnXo4zYFbUirFUME1fB0og5SxSjHZ1arqCe4A=', 'HDipkHp2e6oRzTGCLJayXg==', 'XoqjmjIOn1bTVeaIyRgS2w==', 'p6WsqQDbZ3eCGZXUocpurg==', 1);
/*
	("4532100012345678", "1234", "2028-12-01", "123", 1),
    ("4532100087654321", "4321", "2027-05-01", "456", 1),
    ("5412300011223344", "9876", "2026-08-01", "789", 2),
    ("5412300055667788", "5566", "2029-01-01", "012", 2),
    ("4532100099001122", "1111", "2025-11-01", "345", 1),
    ("4532100033445566", "2222", "2027-03-01", "678", 1),
    ("5412300044556677", "3333", "2028-07-01", "901", 2),
    ("5412300088990011", "4444", "2026-10-01", "234", 2),
    ("4532100022334455", "7788", "2029-06-01", "567", 1),
    ("5412300066778899", "0000", "2027-09-01", "890", 2),

    ("4532100011112222", "1357", "2028-01-01", "111", 1),
    ("4532100033334444", "2468", "2026-02-01", "222", 1),
    ("5412300077778888", "3690", "2029-03-01", "333", 2),
    ("5412300099990000", "9870", "2027-04-01", "444", 2),
    ("4532100044445555", "5555", "2028-05-01", "555", 1),
    ("4532100066667777", "6666", "2026-06-01", "666", 1),
    ("5412300022223333", "7777", "2029-07-01", "777", 2),
    ("5412300044445555", "8888", "2027-08-01", "888", 2),
    ("4532100088889999", "9999", "2028-09-01", "999", 1),
    ("5412300010101010", "1010", "2026-10-01", "101", 2),

    ("4532100020202020", "2020", "2029-11-01", "202", 1),
    ("5412300030303030", "3030", "2027-12-01", "303", 2),
    ("4532100040404040", "4040", "2028-08-01", "404", 1),
    ("5412300050505050", "5050", "2026-04-01", "505", 2),
    ("4532100060606060", "6060", "2029-02-01", "606", 1);
*/

SELECT *
FROM TARJETAS_TB
WHERE TARJETA_TIPO_TARJ_ID = 2;

INSERT INTO CUENTAS_TB ( -- Hacer un Trigger para evitar que se agreguen montos en tarjetas de débito
	CUENTA_TARJETA_ID, 
    CUENTA_MontoAdelantadoEfectivo
) VALUES 
-- 1 a 2 → Débito
(1,  NULL),
(2,  NULL),
-- 3 a 4 → Crédito
(3,  155800.00),
(4,  513000.00),
-- 5 a 6 → Débito
(5,  NULL),
(6,  NULL),
-- 7 a 8 → Crédito
(7,  255050.00),
(8,  500000.00),
-- 9 → Débito
(9,  NULL),
-- 10 → Crédito
(10, 51255.00),
-- 11 a 12 → Débito
(11, NULL),
(12, NULL),
-- 13 a 14 → Crédito
(13, 345000.00),
(14, 410000.00),
-- 15 a 16 → Débito
(15, NULL),
(16, NULL),
-- 17 a 18 → Crédito
(17, 275000.00),
(18, 600000.00),
-- 19 → Débito
(19, NULL),
-- 20 → Crédito
(20, 98000.00),
-- 21 → Débito
(21, NULL),
-- 22 → Crédito
(22, 150000.00),
-- 23 → Débito
(23, NULL),
-- 24 → Crédito
(24, 325000.00),
-- 25 → Débito
(25, NULL);

SELECT *
FROM CUENTAS_TB;

INSERT INTO TIPOS_TRANSACCIONES_TB(
	TIPO_TRANSAC_Nombre
) VALUES 
('Retiro'),
('Consulta'),
('Cambio de PIN');

INSERT INTO ESTADOS_TRANSACCIONES_TB(
	EST_TRANSAC_Nombre
) VALUES
('Pendiente'),
('Completada');

INSERT INTO TRANSACCIONES_TB( -- Si transacción es consulta, cambio de pin entonces monto = 0.00
	TRANSAC_CUENTA_ID,
    TRANSAC_TIPO_TRANSAC_ID,
    TRANSAC_Monto,
    TRANSAC_ESTADO_ID
) VALUES
-- ID_CUENTA, TIPO (1=Retiro, 2=Consulta, 3=PIN), MONTO, ESTADO (1=Pend, 2=Compl)
(1, 1, 500.00, 2),   -- Débito: Completado
(2, 2, 0.00, 2),     -- Consulta: Completado
(3, 1, 1500.00, 1),  -- Crédito: Pendiente (Nueva Regla)
(4, 1, 2000.00, 1),  -- Crédito: Pendiente
(5, 3, 0.00, 2),     -- Cambio PIN: Completado
(6, 1, 300.00, 2),   -- Débito: Completado
(7, 2, 0.00, 2),     -- Consulta
(8, 1, 5000.00, 1),  -- Crédito: Pendiente
(9, 1, 100.00, 2),   -- Débito: Completado
(10, 1, 150.00, 1),  -- Crédito: Pendiente
(1, 2, 0.00, 2),     
(2, 1, 300.00, 2),   -- Débito: Completado
(3, 1, 200.00, 1),   -- Crédito: Pendiente
(4, 3, 0.00, 2),     
(5, 1, 80.00, 2),    -- Débito: Completado
(6, 2, 0.00, 2),     
(7, 1, 1000.00, 1),  -- Crédito: Pendiente
(8, 1, 500.00, 1),   -- Crédito: Pendiente
(9, 3, 0.00, 2),     
(10, 2, 0.00, 2),    
(1, 1, 250.00, 2),   -- Débito: Completado
(3, 1, 350.00, 1),   -- Crédito: Pendiente
(5, 2, 0.00, 2),     
(7, 1, 90.00, 1),    -- Crédito: Pendiente
(8, 2, 0.00, 2);

INSERT INTO CAJEROS_TB(
	CAJ_ID_Identificador
) VALUES
('SIM-001'),
('SIM-002'),
('SIM-003'),
('SIM-004'),
('SIM-005'),
('SIM-006'),
('SIM-007'),
('SIM-008'),
('SIM-009'),
('SIM-010');

INSERT INTO TRANSACCIONES_CAJEROS_TB(
	TRANSAC_CAJ_CAJ_ID,
    TRANSAC_CAJ_TRANSAC_ID,
    TRANSAC_FechaHora
) VALUES
-- Cajero 1 (SIM-001)
(1, 1, '2026-02-06 08:30:00'),
(1, 2, '2026-02-06 08:45:12'),
(1, 3, '2026-02-06 09:10:00'),
(1, 4, '2026-02-06 10:05:22'),
(1, 5, '2026-02-06 10:15:00'),
(1, 6, '2026-02-06 11:00:45'),
-- Cajero 2 (SIM-002)
(2, 7, '2026-02-06 08:15:00'),
(2, 8, '2026-02-06 08:50:30'),
(2, 9, '2026-02-06 09:20:00'),
(2, 10, '2026-02-06 10:40:15'),
(2, 11, '2026-02-06 11:30:00'),
(2, 12, '2026-02-06 12:05:10'),
-- Cajero 3 (SIM-003)
(3, 13, '2026-02-06 07:45:00'),
(3, 14, '2026-02-06 08:22:18'),
(3, 15, '2026-02-06 09:55:00'),
(3, 16, '2026-02-06 10:12:40'),
(3, 17, '2026-02-06 11:20:00'),
(3, 18, '2026-02-06 13:00:05'),
-- Cajero 4 (SIM-004)
(4, 19, '2026-02-06 08:00:00'),
(4, 20, '2026-02-06 09:30:45'),
(4, 21, '2026-02-06 10:50:00'),
(4, 22, '2026-02-06 11:15:20'),
(4, 23, '2026-02-06 12:45:00'),
(4, 24, '2026-02-06 14:10:30'),
(4, 25, '2026-02-06 15:00:00');

-- Reporte para comprobar inserts y datos correctos
SELECT 
    c.CAJ_ID_Identificador AS 'Cajero',
    tt.TIPO_TRANSAC_Nombre AS 'Tipo de Movimiento',
    t.TRANSAC_Monto AS 'Monto',
    et.EST_TRANSAC_Nombre AS 'Estado',
    tc.TRANSAC_FechaHora AS 'Fecha y Hora',
    tar.TARJETA_Numero AS 'Tarjeta (Cifrada)',
    ttj.TIPO_TARJ_Nombre AS 'Tipo de Tarjeta'
FROM TRANSACCIONES_CAJEROS_TB tc
INNER JOIN CAJEROS_TB c ON tc.TRANSAC_CAJ_CAJ_ID = c.CAJ_ID
INNER JOIN TRANSACCIONES_TB t ON tc.TRANSAC_CAJ_TRANSAC_ID = t.TRANSAC_ID
INNER JOIN TIPOS_TRANSACCIONES_TB tt ON t.TRANSAC_TIPO_TRANSAC_ID = tt.TIPO_TRANSAC_ID
INNER JOIN ESTADOS_TRANSACCIONES_TB et ON t.TRANSAC_ESTADO_ID = et.EST_TRANSAC_ID
INNER JOIN CUENTAS_TB cta ON t.TRANSAC_CUENTA_ID = cta.CUENTA_ID
INNER JOIN TARJETAS_TB tar ON cta.CUENTA_TARJETA_ID = tar.TARJETA_ID
INNER JOIN TIPOS_TARJETAS_TB ttj ON tar.TARJETA_TIPO_TARJ_ID = ttj.TIPO_TARJ_ID -- Se cambió _ID por el nombre original
ORDER BY tc.TRANSAC_FechaHora DESC;
    