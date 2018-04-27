using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Relativity.API;

namespace $safeprojectname$
{
	public class Query : IQuery
	{
		public async Task CreateManagerQueueTableAsync(IDBContext eddsDbContext)
		{
			string sql = String.Format(@" 
				IF OBJECT_ID('EDDSDBO.{0}') IS NULL BEGIN
					CREATE TABLE EDDSDBO.{0}
					(
						ID int IDENTITY(1,1) PRIMARY KEY
						,[TimeStampUTC] DATETIME
						,WorkspaceArtifactID INT
						,QueueStatus INT
						,AgentID INT
						,Priority INT
						,UserID INT
						,RecordArtifactID INT
						,ResourceGroupID INT
					)
				END", Constant.Tables.ManagerQueue);

			await Task.Run(() => eddsDbContext.ExecuteNonQuerySQLStatement(sql));
		}

		public async Task CreateWorkerQueueTableAsync(IDBContext eddsDbContext)
		{
			string sql = String.Format(@" 
				IF OBJECT_ID('EDDSDBO.{0}') IS NULL BEGIN
					CREATE TABLE EDDSDBO.{0}
					(
						ID int IDENTITY(1,1) PRIMARY KEY
						,[TimeStampUTC] DATETIME
						,WorkspaceArtifactID INT
						,JobID INT
						,QueueStatus INT
						,AgentID INT
						,RecordArtifactID INT
						,Priority INT
						,ParentRecordArtifactID INT
						,ResourceGroupID INT
					)
				END", Constant.Tables.WorkerQueue);

			await Task.Run(() => eddsDbContext.ExecuteNonQuerySQLStatement(sql));
		}

		public async Task CreateErrorLogTableAsync(IDBContext eddsDbContext)
		{
			string sql = String.Format(@" 
				IF OBJECT_ID('EDDSDBO.{0}') IS NULL BEGIN
					CREATE TABLE EDDSDBO.{0}
					(
						ID int IDENTITY(1,1)
						,[TimeStampUTC] DATETIME
						,WorkspaceArtifactID INT
						,ApplicationName VARCHAR(500)
						,ApplicationGuid uniqueidentifier
						,QueueTableName NVARCHAR(MAX)
						,QueueRecordID INT
						,AgentID INT
						,[Message] NVARCHAR(MAX)
					)
				END", Constant.Tables.ErrorLog);

			await Task.Run(() => eddsDbContext.ExecuteNonQuerySQLStatement(sql));
		}

		public async Task InsertRowIntoErrorLogAsync(IDBContext eddsDbContext, Int32 workspaceArtifactID, String queueTableName, Int32 queueRecordID, Int32 agentId, String errorMessage)
		{
			string sql = String.Format(@" 
			INSERT INTO EDDSDBO.{0}
			(
				[TimeStampUTC]
				,WorkspaceArtifactID
				,ApplicationName
				,ApplicationGuid
				,QueueTableName
				,QueueRecordID
				,AgentID
				,[Message]
			)
			VALUES 
			(
				GetUTCDate()
				,@workspaceArtifactID
				,@applicationName
				,@applicationGuid
				,@queueTableName
				,@queueRecordID
				,@agentID
				,@message
			)", Constant.Tables.ErrorLog);

			List<SqlParameter> sqlParams = new List<SqlParameter>
				{
					new SqlParameter("@workspaceArtifactID", SqlDbType.Int) {Value = workspaceArtifactID},
					new SqlParameter("@applicationName", SqlDbType.VarChar) {Value = Constant.Names.ApplicationName},
					new SqlParameter("@applicationGuid", SqlDbType.UniqueIdentifier) {Value = Constant.Guids.ApplicationGuid},
					new SqlParameter("@queueTableName", SqlDbType.VarChar) {Value = queueTableName},
					new SqlParameter("@queueRecordID", SqlDbType.Int) {Value = queueRecordID},
					new SqlParameter("@agentID", SqlDbType.Int) {Value = agentId},
					new SqlParameter("@message", SqlDbType.NVarChar) {Value = errorMessage}
				};

			await Task.Run(() => eddsDbContext.ExecuteNonQuerySQLStatement(sql, sqlParams));
		}

		public async Task<DataTable> RetrieveNextInManagerQueueAsync(IDBContext eddsDbContext, Int32 agentId, String commaDelimitedResourceAgentIds)
		{
			string sql = String.Format(@" 
				SET NOCOUNT ON 

				DECLARE @ID INT
				DECLARE @WorkspaceArtifactID INT
				DECLARE @Priority INT
				DECLARE @ArtifactID INT
				DECLARE @ResourceGroupID INT

				BEGIN TRAN 
					SELECT TOP 1
							@ID = ID,
							@WorkspaceArtifactID = WorkspaceArtifactID,
							@Priority = Priority,
							@ArtifactID = RecordArtifactID,
							@ResourceGroupID = ResourceGroupID
					FROM eddsdbo.{0} WITH(UPDLOCK,READPAST) 
					WHERE [QueueStatus] = @notStartedQueueStatus
						AND ResourceGroupID IN ({1})
					ORDER BY 
						Priority ASC
						,[TimeStampUTC] ASC

					UPDATE eddsdbo.{0} SET [QueueStatus] = @inProgressQueueStatus, AgentID = @agentId WHERE [ID] = @ID 

				COMMIT 
				SET NOCOUNT OFF 

				SELECT 
					@ID ID,
					@WorkspaceArtifactID WorkspaceArtifactID,
					@Priority Priority,
					@ArtifactID ArtifactID,
					@ResourceGroupID ResourceGroupID
				WHERE @ID IS NOT NULL", Constant.Tables.ManagerQueue, commaDelimitedResourceAgentIds);

			List<SqlParameter> sqlParams = new List<SqlParameter>
				{
					new SqlParameter("@agentID", SqlDbType.Int) {Value = agentId},
					new SqlParameter("@notStartedQueueStatus", SqlDbType.Int) {Value = Constant.QueueStatus.NotStarted},
					new SqlParameter("@inProgressQueueStatus", SqlDbType.Int) {Value = Constant.QueueStatus.InProgress}
				};

			DataTable dt = await Task.Run(() => eddsDbContext.ExecuteSqlStatementAsDataTable(sql, sqlParams));
			return dt;
		}

		public async Task ResetUnfishedJobsAsync(IDBContext eddsDbContext, Int32 agentID, String queueTableName)
		{
			string sql = String.Format(@" 
					UPDATE eddsdbo.{0} SET [QueueStatus] = @notStartedQueueStatus, AgentID = NULL WHERE AgentID = @agentID", queueTableName);

			List<SqlParameter> sqlParams = new List<SqlParameter>
				{
					new SqlParameter("@agentID", SqlDbType.Int) {Value = agentID},
					new SqlParameter("@notStartedQueueStatus", SqlDbType.Int) {Value = Constant.QueueStatus.NotStarted}
				};

			await Task.Run(() => eddsDbContext.ExecuteNonQuerySQLStatement(sql, sqlParams));
		}

		public async Task RemoveRecordFromTableByIDAsync(IDBContext eddsDbContext, String queueTableName, Int32 id)
		{
			string sql = String.Format(@" 
				DELETE FROM EDDSDBO.{0}
				WHERE ID = @id", queueTableName);

			List<SqlParameter> sqlParams = new List<SqlParameter>
				{
					new SqlParameter("@id", SqlDbType.Int) {Value = id}
				};

			await Task.Run(() => eddsDbContext.ExecuteNonQuerySQLStatement(sql, sqlParams));
		}

		public async Task InsertRowsIntoWorkerQueueAsync(IDBContext eddsDbContext, Int32 jobId, Int32 priority, Int32 workspaceArtifactId, Int32 parentRecordArtifactId, Int32 resourceGroupId)
		{
			string sql = String.Format(@" 
				INSERT INTO EDDSDBO.{0}
				(
					[TimeStampUTC] 
					,WorkspaceArtifactID
					,JobID
					,QueueStatus
					,AgentID
					,RecordArtifactID
					,Priority
					,ParentRecordArtifactID
					,ResourceGroupID
				)
				SELECT 
					@timeStamp
					,@workspaceArtifactID
					,@jobID
					,@notStartedQueueStatus
					,NULL
					,123
					,@priority
					,@parentRecordArtifactID
					,@resourceGroupID
				UNION ALL
				SELECT 
					@timeStamp
					,@workspaceArtifactID
					,@jobID
					,@notStartedQueueStatus
					,NULL
					,456
					,@priority
					,@parentRecordArtifactID
					,@resourceGroupID
				UNION ALL
				SELECT 
					@timeStamp
					,@workspaceArtifactID
					,@jobID
					,@notStartedQueueStatus
					,NULL
					,789
					,@priority
					,@parentRecordArtifactID
					,@resourceGroupID
				", Constant.Tables.WorkerQueue);

			List<SqlParameter> sqlParams = new List<SqlParameter>
				{
					new SqlParameter("@jobID", SqlDbType.Int) {Value = jobId},
					new SqlParameter("@priority", SqlDbType.Int) {Value = priority},
					new SqlParameter("@workspaceArtifactID", SqlDbType.Int) {Value = workspaceArtifactId},
					new SqlParameter("@timeStamp", SqlDbType.DateTime) {Value = DateTime.UtcNow},
					new SqlParameter("@parentRecordArtifactID", SqlDbType.Int) {Value = parentRecordArtifactId},
					new SqlParameter("@notStartedQueueStatus", SqlDbType.Int) {Value = Constant.QueueStatus.NotStarted},
					new SqlParameter("@resourceGroupID", SqlDbType.Int) {Value = resourceGroupId}
				};

			await Task.Run(() => eddsDbContext.ExecuteNonQuerySQLStatement(sql, sqlParams));
		}

		public async Task UpdateStatusInManagerQueueAsync(IDBContext eddsDbContext, Int32 statusId, Int32 id)
		{
			string sql = String.Format(@" 
				UPDATE EDDSDBO.{0} SET QueueStatus = @statusId WHERE ID = @id", Constant.Tables.ManagerQueue);

			List<SqlParameter> sqlParams = new List<SqlParameter>
				{
					new SqlParameter("@statusId", SqlDbType.Int) {Value = statusId},
					new SqlParameter("@id", SqlDbType.Int) {Value = id}
				};

			await Task.Run(() => eddsDbContext.ExecuteNonQuerySQLStatement(sql, sqlParams));
		}

		public async Task UpdateStatusInWorkerQueueAsync(IDBContext eddsDbContext, Int32 statusId, String uniqueTableName)
		{
			string sql = String.Format(@" 
					UPDATE S 
						SET QueueStatus = @statusId
					FROM EDDSDBO.{1} B
						INNER JOIN EDDSDBO.{0} S ON B.ID = S.ID", Constant.Tables.WorkerQueue, uniqueTableName);

			List<SqlParameter> sqlParams = new List<SqlParameter>
				{
					new SqlParameter("@statusId", SqlDbType.Int) {Value = statusId}
				};

			await Task.Run(() => eddsDbContext.ExecuteNonQuerySQLStatement(sql, sqlParams));
		}

		public async Task<DataTable> RetrieveNextBatchInWorkerQueueAsync(IDBContext eddsDbContext, Int32 agentId, Int32 batchSize, String uniqueTableName, String commaDelimitedResourceAgentIds)
		{
			string sql = String.Format(@" 
				BEGIN TRAN
					IF NOT OBJECT_ID('EDDSDBO.{1}') IS NULL BEGIN
						DROP TABLE EDDSDBO.{1}
					END
					CREATE TABLE EDDSDBO.{1}(ID INT)
					
					DECLARE @jobId INT 
					SET @jobId = 
					(
						SELECT TOP 1 JobID
						FROM EDDSDBO.{0}
						WHERE QueueStatus = @notStartedQueueStatus 
							AND ResourceGroupID IN ({2})
						ORDER BY 
							Priority ASC
							,[TimeStampUTC] ASC
					)

					UPDATE eddsdbo.{0}
					SET AgentID = @agentID, 
						QueueStatus = @inProgressQueueStatus
					OUTPUT inserted.ID
					INTO EDDSDBO.{1}(ID) 
					FROM EDDSDBO.{0} WITH(UPDLOCK,READPAST) 
					WHERE ID IN
							(
								SELECT TOP (@batchSize) ID
								FROM EDDSDBO.{0} WITH(UPDLOCK,READPAST) 
								WHERE JobID = @jobId 
									AND QueueStatus = @notStartedQueueStatus
								ORDER BY 
									Priority ASC
									,[TimeStampUTC] ASC 
							)		
				COMMIT

				SELECT
					S.ID QueueID
					,S.WorkspaceArtifactID
					,S.Priority
					,S.RecordArtifactID ArtifactID
					,S.JobID ID
					,S.ResourceGroupID
				FROM EDDSDBO.{1} B
					INNER JOIN eddsdbo.{0} S ON B.ID = S.ID	", Constant.Tables.WorkerQueue, uniqueTableName, commaDelimitedResourceAgentIds);

			List<SqlParameter> sqlParams = new List<SqlParameter>
				{
					new SqlParameter("@agentID", SqlDbType.Int) {Value = agentId},
					new SqlParameter("@batchSize", SqlDbType.Int) {Value = batchSize},
					new SqlParameter("@notStartedQueueStatus", SqlDbType.Int) {Value = Constant.QueueStatus.NotStarted},
					new SqlParameter("@inProgressQueueStatus", SqlDbType.Int) {Value = Constant.QueueStatus.InProgress}
				};

			return await Task.Run(() => eddsDbContext.ExecuteSqlStatementAsDataTable(sql, sqlParams));
		}

		public async Task RemoveBatchFromQueueAsync(IDBContext eddsDbContext, String uniqueTableName)
		{
			string sql = String.Format(@"  
				DELETE EDDSDBO.{0}
				FROM EDDSDBO.{0} S
					INNER JOIN EDDSDBO.{1} B ON B.ID = S.ID	", Constant.Tables.WorkerQueue, uniqueTableName);

			await Task.Run(() => eddsDbContext.ExecuteNonQuerySQLStatement(sql));
		}

		public async Task DropTableAsync(IDBContext dbContext, String tableName)
		{
			string sql = String.Format(@"  
				IF NOT OBJECT_ID('EDDSDBO.{0}') IS NULL 
					BEGIN DROP TABLE EDDSDBO.{0}
				END", tableName);

			await Task.Run(() => dbContext.ExecuteNonQuerySQLStatement(sql));
		}

		public async Task<DataTable> RetrieveAllInManagerQueueAsync(IDBContext dbContext)
		{
			string sql = String.Format(@" 

				DECLARE @offset INT SET @offset = (SELECT DATEDIFF(HOUR,GetUTCDate(),GetDate()))

				SELECT 
					Q.[ID]
					,DATEADD(HOUR,@offset,Q.[TimeStampUTC]) [Added On]
					,Q.WorkspaceArtifactID [Workspace Artifact ID]
					,C.Name [Workspace Name]
					,CASE Q.[QueueStatus]	
						WHEN @notStartedStatusId THEN 'Waiting'
						WHEN @inProgressStatusId THEN 'In Progress'
						WHEN @errorStatusId THEN 'Error'
						END [Status]
					,Q.AgentID [Agent Artifact ID]
					,Q.[Priority]
					,U.LastName + ', ' + U.FirstName [Added By]
					,Q.RecordArtifactID [Record Artifact ID]
				FROM EDDSDBO.{0} Q
					INNER JOIN EDDS.EDDSDBO.ExtendedCase C ON Q.WorkspaceArtifactID = C.ArtifactID
					LEFT JOIN EDDS.EDDSDBO.[User] U ON Q.UserID = U.ArtifactID
				ORDER BY 
					Q.[Priority] ASC
					,Q.[TimeStampUTC] ASC", Constant.Tables.ManagerQueue);

			List<SqlParameter> sqlParams = new List<SqlParameter>
				{
					new SqlParameter("@notStartedStatusId", SqlDbType.Int) {Value = Constant.QueueStatus.NotStarted},
					new SqlParameter("@inProgressStatusId", SqlDbType.Int) {Value = Constant.QueueStatus.InProgress},
					new SqlParameter("@errorStatusId", SqlDbType.Int) {Value = Constant.QueueStatus.Error}
				};

			return await Task.Run(() => dbContext.ExecuteSqlStatementAsDataTable(sql, sqlParams));
		}

		public async Task<DataRow> RetrieveSingleInManagerQueueByArtifactIdAsync(IDBContext dbContext, Int32 artifactId, Int32 workspaceArtifactId)
		{
			string sql = String.Format(@" 

				DECLARE @offset INT SET @offset = (SELECT DATEDIFF(HOUR,GetUTCDate(),GetDate()))

				SELECT 
					Q.[ID]
					,DATEADD(HOUR,@offset,Q.[TimeStampUTC]) [Added On]
					,Q.WorkspaceArtifactID [Workspace Artifact ID]
					,C.Name [Workspace Name]
					,CASE Q.[QueueStatus]	
						WHEN @notStartedStatusId THEN 'Waiting'
						WHEN @inProgressStatusId THEN 'In Progress'
						WHEN @errorStatusId THEN 'Error'
						END [Status]
					,Q.AgentID [Agent Artifact ID]
					,Q.[Priority]
					,U.LastName + ', ' + U.FirstName [Added By]
					,Q.RecordArtifactID [Record Artifact ID]
				FROM EDDSDBO.{0} Q
					INNER JOIN EDDS.EDDSDBO.ExtendedCase C ON Q.WorkspaceArtifactID = C.ArtifactID
					LEFT JOIN EDDS.EDDSDBO.[User] U ON Q.UserID = U.ArtifactID
				WHERE Q.RecordArtifactID = @artifactId
					AND Q.WorkspaceArtifactID = @workspaceArtifactId", Constant.Tables.ManagerQueue);

			List<SqlParameter> sqlParams = new List<SqlParameter>
				{
					new SqlParameter("@notStartedStatusId", SqlDbType.Int) {Value = Constant.QueueStatus.NotStarted},
					new SqlParameter("@inProgressStatusId", SqlDbType.Int) {Value = Constant.QueueStatus.InProgress},
					new SqlParameter("@errorStatusId", SqlDbType.Int) {Value = Constant.QueueStatus.Error},
					new SqlParameter("@artifactId", SqlDbType.Int) {Value = artifactId},
					new SqlParameter("@workspaceArtifactId", SqlDbType.Int) {Value = workspaceArtifactId}
				};

			DataTable dt = await Task.Run(() => dbContext.ExecuteSqlStatementAsDataTable(sql, sqlParams));
			if (dt.Rows.Count > 0)
			{
				return dt.Rows[0];
			}
			return null;
		}

		public async Task<DataTable> RetrieveAllInWorkerQueueAsync(IDBContext dbContext)
		{
			string sql = String.Format(@" 

				DECLARE @offset INT SET @offset = (SELECT DATEDIFF(HOUR,GetUTCDate(),GetDate()))

				SELECT 
					Q.JobID [ID]
					,DATEADD(HOUR,@offset,Q.[TimeStampUTC]) [Added On]
					,Q.WorkspaceArtifactID [Workspace Artifact ID]
					,C.Name [Workspace Name]
					,CASE Q.[QueueStatus]	
						WHEN @notStartedStatusId THEN 'Waiting'
						WHEN @inProgressStatusId THEN 'In Progress'
						WHEN @errorStatusId THEN 'Error'
						END [Status]
					,Q.AgentID [Agent Artifact ID]
					,Q.[Priority]
					,COUNT(Q.[ID]) [# Records Remaining]
					,Q.ParentRecordArtifactID [Parent Record Artifact ID]
				FROM EDDSDBO.{0} Q
					INNER JOIN EDDS.EDDSDBO.ExtendedCase C ON Q.WorkspaceArtifactID = C.ArtifactID
				GROUP BY 
					Q.JobID
					,Q.[TimeStampUTC]
					,C.Name
					,Q.WorkspaceArtifactID
					,Q.[QueueStatus]
					,Q.AgentID
					,Q.[Priority]
					,Q.ParentRecordArtifactID
				ORDER BY 
					Q.[Priority] ASC
					,Q.[TimeStampUTC] ASC
					,Q.JobID ASC
					,Q.[QueueStatus] DESC", Constant.Tables.WorkerQueue);

			List<SqlParameter> sqlParams = new List<SqlParameter>
				{
					new SqlParameter("@notStartedStatusId", SqlDbType.Int) {Value = Constant.QueueStatus.NotStarted},
					new SqlParameter("@inProgressStatusId", SqlDbType.Int) {Value = Constant.QueueStatus.InProgress},
					new SqlParameter("@errorStatusId", SqlDbType.Int) {Value = Constant.QueueStatus.Error}
				};

			return await Task.Run(() => dbContext.ExecuteSqlStatementAsDataTable(sql, sqlParams));
		}

		public async Task InsertRowIntoManagerQueueAsync(IDBContext eddsDbContext, Int32 workspaceArtifactId, Int32 priority, Int32 userId, Int32 artifactId, Int32 resourceGroupId)
		{
			string sql = String.Format(@" 
			INSERT INTO EDDSDBO.{0}
			(
				[TimeStampUTC]
				,WorkspaceArtifactID
				,QueueStatus
				,AgentID
				,[Priority]
				,[UserID]
				,RecordArtifactID
				,ResourceGroupID
			)
			VALUES 
			(
				GetUTCDate()
				,@workspaceArtifactId
				,@queueStatus
				,NULL
				,@priority
				,@userID
				,@artifactID
				,@resourceGroupID
			)", Constant.Tables.ManagerQueue);

			List<SqlParameter> sqlParams = new List<SqlParameter>
				{
					new SqlParameter("@workspaceArtifactId", SqlDbType.Int) {Value = workspaceArtifactId},
					new SqlParameter("@queueStatus", SqlDbType.VarChar) {Value = Constant.QueueStatus.NotStarted},
					new SqlParameter("@priority", SqlDbType.Int) {Value = priority},
					new SqlParameter("@userID", SqlDbType.Int) {Value = userId},
					new SqlParameter("@artifactID", SqlDbType.Int) {Value = artifactId},
					new SqlParameter("@resourceGroupID", SqlDbType.Int) {Value = resourceGroupId}
				};

			await Task.Run(() => eddsDbContext.ExecuteNonQuerySQLStatement(sql, sqlParams));
		}

		public async Task<DataTable> RetrieveOffHoursAsync(IDBContext eddsDbContext)
		{
			string sql = @"
				DECLARE @OffHourStart VARCHAR(100)
				DECLARE @OffHourEndTime VARCHAR(100)

				SET @OffHourStart = (SELECT [VALUE] FROM [EDDS].[eddsdbo].[Configuration] WITH(NOLOCK) WHERE [SECTION] = 'kCura.EDDS.Agents' AND [NAME] = 'AgentOffHourStartTime')
				SET @OffHourEndTime = (SELECT [VALUE] FROM [EDDS].[eddsdbo].[Configuration] WITH(NOLOCK) WHERE [SECTION] = 'kCura.EDDS.Agents' AND [NAME] = 'AgentOffHourEndTime')

				SELECT
					@OffHourStart AS [AgentOffHourStartTime],
					@OffHourEndTime AS [AgentOffHourEndTime]
				";

			DataTable dt = await Task.Run(() => eddsDbContext.ExecuteSqlStatementAsDataTable(sql));
			if (dt.Rows.Count > 0)
			{
				return dt;
			}
			return null;
		}
	}

}

