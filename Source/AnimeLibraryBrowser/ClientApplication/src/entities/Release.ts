import FileGroup from "./FileGroup";

interface Release {
  title: string;
  year: number;
  type: string;
  frameWidth: number;
  frameHeight: number;
  videoEncoder: string;
  audioEncoder: string;
  fileGroups?: FileGroup[];
}

export default Release;
