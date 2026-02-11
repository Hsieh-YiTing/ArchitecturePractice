WITH AllMatchingData AS (
    SELECT r.RV01 AS 預約序號,
           pd.PD02 AS 套餐名稱,
           hp.PAT02 AS 姓名,
           (CASE
                WHEN hp.PAT08 = 1
                THEN '男'
                ELSE '女' 
            END) AS 性別,
           FORMAT(hp.PAT04, 'yyyy-MM-dd') AS 出生日期,
           hp.PAT03 AS 身份證字號,
           hp.PAT07 AS 病歷號碼,
           hp.PAT05 AS 電話,
           hp.PAT06 AS 地址,
		   r.RV02 AS 健檢日期,
           ri.RI01 AS 預約細項序號,
           ri.RI08 AS 檢查結果,
           ri.RI09 AS 檢驗評值, 
           (CASE 
                WHEN di.DI19 = 0 THEN di.DI09
                WHEN di.DI19 IN (1, 2, 3) THEN ri.RI29
            END) AS 單位,
           di.DI02 AS 細項名稱,  
           (CASE
                WHEN di.DI19 = 0 THEN di.DI25 
                WHEN di.DI19 IN (1, 2, 3) THEN
                    CASE hp.PAT08
                        WHEN 1 THEN ri.RI27
                        WHEN 2 THEN ri.RI28
                    END
            END) AS 正常值_中,
           di.DI05 AS 報告類型,
           ic.IC01 AS 細項類別序號, 
           ic.IC02 AS 細項類別名稱,
           rc.RC05 AS 是否列印細項參考值,
           io.IO02 AS 器官名稱,
           ad.AD05 AS 負責醫師,
           ad.AD07 AS 醫師證號,
           ad.AD10 AS 醫師章授權,
           DENSE_RANK() OVER (ORDER BY r.RV02 DESC) AS DateRank
    FROM Reservation r
    JOIN HH_MPAT hp ON r.PAT03 = hp.PAT03 AND r.PAT07 = hp.PAT07
    JOIN ReservationItem ri ON r.RV01 = ri.RV01
    JOIN DetailItem di ON ri.DI03 = di.DI03
    JOIN ItemOrgan io ON di.DI07 = io.IO01
    JOIN ItemClass ic ON di.IC01 = ic.IC01
    JOIN ReportClass rc ON ic.RC01 = rc.RC01
    JOIN AccountDetail ad ON r.RV20 = ad.AD01
    LEFT JOIN PackageDetail pd ON ri.PD01 = pd.PD01
    WHERE RI08 IS NOT NULL 
		  AND RI003 = 0 
		  AND DI003 = 0 
		  AND IC003 = 0 
		  AND PD003 = 0
		  /* CTE_WHERE_CLAUSE */
)
SELECT amd.*
FROM AllMatchingData amd
/* WHERE_CLAUSE_BY_EXPORT_TYPE */
ORDER BY amd.DateRank, amd.細項名稱;