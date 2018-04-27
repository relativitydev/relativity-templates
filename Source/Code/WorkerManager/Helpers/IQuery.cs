using System;
using System.Data;
using System.Threading.Tasks;
using Relativity.API;

namespace Helpers
{
	public interface IQuery
	{
		Task CreateManagerQueueTableAsync(IDBContext eddsDbContext);

		Task CreateWorkerQueueTableAsync(IDBContext eddsDbContext);

		Task CreateErrorLogTableAsync(IDBContext eddsDbContext);

		Task InsertRowIntoErrorLogAsync(IDBContext eddsDbContext, Int32 workspaceArtifactID, String queueTableName, Int32 queueRecordID, Int32 agentId, String errorMessage);

		Task<DataTable> RetrieveNextInManagerQueueAsync(IDBContext eddsDbContext, Int32 agentId, String commaDelimitedResourceAgentIds);

		Task ResetUnfishedJobsAsync(IDBContext eddsDbContext, Int32 agentID, String queueTableName);

		Task RemoveRecordFromTableByIDAsync(IDBContext eddsDbContext, String queueTableName, Int32 id);

		Task InsertRowsIntoWorkerQueueAsync(IDBContext eddsDbContext, Int32 jobId, Int32 priority, Int32 workspaceArtifactId, Int32 parentRecordArtifactId, Int32 resourceGroupId);

		Task UpdateStatusInManagerQueueAsync(IDBContext eddsDbContext, Int32 statusId, Int32 id);

		Task UpdateStatusInWorkerQueueAsync(IDBContext eddsDbContext, Int32 statusId, String uniqueTableName);

		Task<DataTable> RetrieveNextBatchInWorkerQueueAsync(IDBContext eddsDbContext, Int32 agentId, Int32 batchSize, String uniqueTableName, String commaDelimitedResourceAgentIds);

		Task RemoveBatchFromQueueAsync(IDBContext eddsDbContext, String uniqueTableName);

		Task DropTableAsync(IDBContext dbContext, String tableName);

		Task<DataTable> RetrieveAllInManagerQueueAsync(IDBContext dbContext);

		Task<DataRow> RetrieveSingleInManagerQueueByArtifactIdAsync(IDBContext dbContext, Int32 artifactId, Int32 workspaceArtifactId);

		Task<DataTable> RetrieveAllInWorkerQueueAsync(IDBContext dbContext);

		Task InsertRowIntoManagerQueueAsync(IDBContext eddsDbContext, Int32 workspaceArtifactId, Int32 priority, Int32 userId, Int32 artifactId, Int32 resourceGroupId);

		Task<DataTable> RetrieveOffHoursAsync(IDBContext eddsDbContext);
	}
}
