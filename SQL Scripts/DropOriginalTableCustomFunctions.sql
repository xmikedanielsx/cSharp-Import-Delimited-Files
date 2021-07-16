IF EXISTS(SELECT 1 FROM sys.procedures p WHERE name = 'get_OriginalUnion')
  BEGIN
	DROP PROCEDURE [get_OriginalUnion]
  END

IF EXISTS(SELECT * FROM sys.table_types tt WHERE name = 'OriginalTableList' AND tt.is_user_defined = 1)
  BEGIN
	DROP TYPE [OriginalTableList] 
  END