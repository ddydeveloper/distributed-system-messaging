import os
import pathlib
import subprocess
import sys
import time

import yaml


class Configuration:
    def __init__(self):
        mode = sys.argv[1]

        build_folder = pathlib.Path(__file__).parent
        config_path = self.__get_path(build_folder, f"{mode}.yaml")

        self._root = build_folder.parent

        with open(config_path) as f:
            self._config = yaml.load(f, Loader=yaml.FullLoader)

    @property
    def producer_cmd(self):
        file = self.__get_path(self._root, self._config["producer_path"])
        return f"dotnet run -p \"{file}\""

    @property
    def consumer_cmd(self):
        file = self.__get_path(self._root, self._config["consumer_path"])
        return f"dotnet run -p \"{file}\""

    @property
    def consumers(self):
        return self._config["consumers"]

    @property
    def messages(self):
        return self._config["messages"]

    @staticmethod
    def __get_path(root, path):
        return pathlib.Path(root).joinpath(path).resolve()


class Runner:
    def __init__(self, config: Configuration):
        self._config = config
        self.__exec_cmd()

    def __publish(self):
        cmd = ""
        for msg in self._config.messages:
            cmd += f"{self._config.producer_cmd} {msg} & "

        res = cmd[:-2]
        return res

    def __consume(self):
        cmd = ""
        for args in self._config.consumers:
            cmd += f"{self._config.consumer_cmd} {args}; split-pane "

        return cmd[:-13]

    def __exec_cmd(self):
        os.system(f"wt {self.__consume()}")
        time.sleep(5)

        publish_cmd = self.__publish()
        subprocess.Popen(publish_cmd, shell=True)


if __name__ == '__main__':
    runner = Runner(Configuration())
