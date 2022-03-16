import FileType from "./FileType";

interface File {
  name: string;
  type: FileType;
  length: number;
  ftpLink: string;
  relativePath: string;
}

export default File;
