/*
 * MSR Tools - tools for mining software repositories
 * 
 * Copyright (C) 2010  Semyon Kirnosenko
 */

using System;
using System.Collections.Generic;
using System.Linq;

using MSR.Data.VersionControl;
using MSR.Data.Entities.DSL.Mapping;
using MSR.Data.Entities.DSL.Selection;

namespace MSR.Data.Entities.Mapping
{
	public class CodeBlockMapperBasedOnBlame : EntityMapper<CodeBlock,ModificationMappingExpression,CodeBlockMappingExpression>
	{
		public CodeBlockMapperBasedOnBlame(IScmData scmData)
			: base(scmData)
		{
		}
		public override IEnumerable<CodeBlockMappingExpression> Map(ModificationMappingExpression expression)
		{
			List<CodeBlockMappingExpression> codeBlockExpressions = new List<CodeBlockMappingExpression>();
			string revision = expression.CurrentEntity<Commit>().Revision;
			ProjectFile file = expression.CurrentEntity<ProjectFile>();
			bool fileIsNew = (file.AddedInCommit != null) && (file.AddedInCommit.Revision == revision);
			bool fileCopied = fileIsNew && (file.SourceFile != null);
			bool fileDeleted = file.DeletedInCommit != null;
			
			if (fileDeleted)
			{
				codeBlockExpressions.Add(
					expression.DeleteCode()
				);
			}
			else
			{
				IBlame blame = scmData.Blame(revision, file.Path);
				var linesByRevision = from l in blame group l.Key by l.Value;
				
				if (fileCopied)
				{
					foreach (var linesForRevision in linesByRevision)
					{
						codeBlockExpressions.Add(
							expression.Code(linesForRevision.Count())
						);
						if (linesForRevision.Key != revision)
						{
							codeBlockExpressions.Last().CopiedFrom(linesForRevision.Key);
						}
					}
				}
				else
				{
					var addedCode = linesByRevision.SingleOrDefault(x => x.Key == revision);
					if (addedCode != null)
					{
                        ///////////////////////////////////////////////////

                        int linesNumber=addedCode.Count();
                        int[] lines = new int [linesNumber];
                        for (int i = 0; i < linesNumber; i++)
                            lines[i] = addedCode.ElementAt(i);
                        string hashTree = scmData.CatFile_Commit(revision);
                        string[] parts = file.Path.Split('/');
                        int partsNumber = parts.Count();
                        for (int i = 1; i < partsNumber; i++)
                        {
                            hashTree = scmData.CatFile_Tree(hashTree, parts[i]);
                        }
                        string hashBlob = hashTree;
                        int commentLinesNumber = scmData.CatFile_Blob(hashBlob, lines, linesNumber);

                        ///////////////////////////////////////////////////
						codeBlockExpressions.Add(
                            expression.Code(linesNumber - commentLinesNumber) //���-�� ����� ����� ���-�� ����� � �������������
						);
					}
					
					foreach (var existentCode in (
						from cb in expression.Queryable<CodeBlock>()
						join m in expression.Queryable<Modification>() on cb.ModificationID equals m.ID
						join f in expression.Queryable<ProjectFile>() on m.FileID equals f.ID
						join c in expression.Queryable<Commit>() on m.CommitID equals c.ID
							let addedCodeID = cb.Size < 0 ? cb.TargetCodeBlockID : cb.ID
							let addedCodeRevision = expression.Queryable<Commit>()
								.Single(x => x.ID == expression.Queryable<CodeBlock>()
									.Single(y => y.ID == addedCodeID).AddedInitiallyInCommitID
								).Revision
						where
							f.ID == file.ID
						group cb.Size by addedCodeRevision into g
						select new
						{
							Revision = g.Key,
							CodeSize = g.Sum()
						}
					))
					{
						var linesForRevision = linesByRevision.SingleOrDefault(x => x.Key == existentCode.Revision);
                        ///////////////////////////////////////////////////

                        int linesNumber = linesForRevision.Count();
                        int[] lines = new int[linesNumber];
                        for (int i = 0; i < linesNumber; i++)
                            lines[i] = linesForRevision.ElementAt(i);
                        string hashTree = scmData.CatFile_Commit(existentCode.Revision);
                        string[] parts = file.Path.Split('/');
                        int partsNumber = parts.Count();
                        for (int i = 1; i < partsNumber; i++)
                        {
                            hashTree = scmData.CatFile_Tree(hashTree, parts[i]);
                        }
                        string hashBlob = hashTree;
                        int commentLinesNumber = scmData.CatFile_Blob(hashBlob, lines, linesNumber);

                        ///////////////////////////////////////////////////
						double realCodeSize = linesForRevision == null ? 0 : linesForRevision.Count();
                        realCodeSize -= commentLinesNumber;
						if (existentCode.CodeSize > realCodeSize)
						{
							codeBlockExpressions.Add(
								expression.Code(realCodeSize - existentCode.CodeSize)
							);
							codeBlockExpressions.Last().ForCodeAddedInitiallyInRevision(existentCode.Revision);
						}
					}
				}
			}
			
			return codeBlockExpressions;
		}
	}
}
