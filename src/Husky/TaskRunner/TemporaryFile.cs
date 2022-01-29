using System.IO.Abstractions;

namespace Husky.TaskRunner;

public sealed class TemporaryFile : IDisposable
{
   private readonly IFileSystem _fileSystem;
   private readonly string _filePath;

   public TemporaryFile(IFileSystem fileSystem, FileArgumentInfo fileArgumentInfo)
   {
      _fileSystem = fileSystem;

      var path = fileArgumentInfo.PathMode == PathModes.Absolute
         ? fileArgumentInfo.AbsolutePath
         : fileArgumentInfo.RelativePath;

      var fileInfo = _fileSystem.FileInfo.FromFileName(path);
      var guid = Guid.NewGuid().ToString()[..5];
      _filePath = path.Replace(fileInfo.Name, $"{guid}_{fileInfo.Name}");
   }

   public static implicit operator string(TemporaryFile temporaryFile) => temporaryFile._filePath;

   public void Dispose()
   {
      if (_fileSystem.File.Exists(_filePath))
         _fileSystem.File.Delete(_filePath);
   }

   public override string ToString()
   {
      return _filePath;
   }
}
