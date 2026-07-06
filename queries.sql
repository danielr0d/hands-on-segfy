-- Analytical queries for the Segfy Sinistros database (PostgreSQL).
-- Identifiers are double-quoted because EF Core (Npgsql provider) preserves the
-- PascalCase table/column names verbatim instead of folding them to lowercase.

-- 1. Ranking of insurance branches (Ramos) by percentage of denied claims (Negado)
--    among claims opened in the last 6 months.
SELECT
    r."Id"                                                 AS ramo_id,
    r."Nome"                                                AS ramo_nome,
    COUNT(s."Id")                                           AS total_sinistros,
    COUNT(s."Id") FILTER (WHERE s."Status" = 'Negado')      AS sinistros_negados,
    ROUND(
        100.0 * COUNT(s."Id") FILTER (WHERE s."Status" = 'Negado') / NULLIF(COUNT(s."Id"), 0),
        2
    )                                                        AS percentual_negados
FROM "Ramos" r
LEFT JOIN "Apolices" a ON a."RamoId" = r."Id"
LEFT JOIN "Sinistros" s ON s."ApoliceId" = a."Id" AND s."DataAbertura" >= NOW() - INTERVAL '6 months'
GROUP BY r."Id", r."Nome"
ORDER BY percentual_negados DESC NULLS LAST;

-- 2. Top 10 clients by sum of ValorEstimado across claims currently EmAnalise or Aprovado.
SELECT
    c."Id"                  AS cliente_id,
    c."Nome"                 AS cliente_nome,
    SUM(s."ValorEstimado")   AS soma_valor_estimado
FROM "Clientes" c
JOIN "Apolices" a  ON a."ClienteId" = c."Id"
JOIN "Sinistros" s ON s."ApoliceId" = a."Id"
WHERE s."Status" IN ('EmAnalise', 'Aprovado')
GROUP BY c."Id", c."Nome"
ORDER BY soma_valor_estimado DESC
LIMIT 10;

-- 3. Average resolution time (in days) of closed claims (Encerrado), grouped by branch.
--    Resolution time = the moment HistoricoSinistros records the transition to
--    Encerrado minus DataAbertura (Encerrado is a terminal status, so there is at
--    most one such history row per claim).
SELECT
    r."Id"   AS ramo_id,
    r."Nome" AS ramo_nome,
    ROUND(
        AVG(EXTRACT(EPOCH FROM (h."DataAlteracao" - s."DataAbertura")) / 86400.0),
        2
    ) AS media_dias_resolucao
FROM "Ramos" r
JOIN "Apolices" a           ON a."RamoId" = r."Id"
JOIN "Sinistros" s          ON s."ApoliceId" = a."Id"
JOIN "HistoricoSinistros" h ON h."SinistroId" = s."Id" AND h."StatusNovo" = 'Encerrado'
GROUP BY r."Id", r."Nome"
ORDER BY media_dias_resolucao;
