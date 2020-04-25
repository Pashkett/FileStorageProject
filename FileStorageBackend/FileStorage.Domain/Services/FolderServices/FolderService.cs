using AutoMapper;
using FileStorage.Data.Models;
using FileStorage.Data.UnitOfWork;
using FileStorage.Domain.DataTransferedObjects.StorageItemModels;
using FileStorage.Domain.UserModels;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FileStorage.Domain.Services.FolderServices
{
    public class FolderService : IFolderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public FolderService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        public async Task<StorageItem> CreateFolderAsync(FolderCreateRequestDto folderCreate)
        {
            var folder = mapper.Map<FolderCreateRequestDto, StorageItem>(folderCreate);
            SetFolderTrustedNameAndPath(folder);

            var existingFolderInDirectory = 
                await unitOfWork.StorageItems.FindByTrustedNameAsync(folder.TrustedName);

            if (existingFolderInDirectory == null)
            {
                await unitOfWork.StorageItems.AddAsync(folder);
                await unitOfWork.CommitAsync();

                return folder;
            }
            else
            {
                throw new ArgumentException("Folder has been already exists!");
            }
        }

        private void SetFolderTrustedNameAndPath(StorageItem folder)
        {
            folder.TrustedName = folder.User.Id.GetHashCode().ToString();

            if (folder.IsFolder)
            {
                if (folder.IsRootFolder == null)
                {
                    throw new ArgumentNullException($"IsRootFolder property is null for {folder.DisplayName}");
                    //TODO logging
                }
                if (folder?.IsRootFolder == true)
                {
                    folder.TrustedName = folder.User.Id.GetHashCode().ToString();
                    folder.RelativePath = folder.TrustedName;

                    return;
                }

                if (folder?.IsRootFolder == false)
                {
                    folder.TrustedName = string.Concat(folder.User.Id.GetHashCode().ToString(),
                                                       DateTime.Now.ToString());
                    folder.RelativePath = Path.Combine(folder.User.Id.GetHashCode().ToString(), 
                                                       folder.TrustedName);

                    return;
                }
            }
            else
            {
                throw new ArgumentException("Record should be a folder");
            }
        }
    }
}
