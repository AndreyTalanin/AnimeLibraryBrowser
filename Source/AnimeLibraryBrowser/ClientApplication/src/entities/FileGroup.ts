import File from "./File";

interface FileGroup {
  name: string;
  relativePath: string;
  files: File[];
}

export default FileGroup;
