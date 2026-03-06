using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;

namespace NetDream.Shared.Interfaces
{
    public interface ITeamRepository
    {
        public IListLabelItem[] Get(int[] idItems);

        public IUser[] Users(int team, int[] userItems);

        public LinkExtraRule[] At(ILinkRuler ruler, string content, int team);
    }
}
