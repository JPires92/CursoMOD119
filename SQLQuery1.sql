USE "aspnet-CursoMOD119-dcac2081-bd7f-4ab6-b761-697282085e4f";

SELECT * FROM Movie;


EXEC sp_databases;




CREATE DATABASE [Test_DB];

USE Test_DB;

CREATE TABLE Artigo (
	id				INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	nome			VARCHAR(255) NOT NULL,
	preco			MONEY,
	dataCriacao		DATE NOT NULL,
	descontinuado	BIT NOT NULL

);

DROP TABLE Artigo;


SELECT * FROM Artigo;

INSERT INTO Artigo
(nome, preco, dataCriacao, descontinuado)
VALUES
('Meias Batáguas', 5.00, '2023-01-01', 0);

UPDATE Artigo
SET 