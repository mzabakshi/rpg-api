using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rpg_api.Data;
using rpg_api.Dtos.Fight;

namespace rpg_api.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _context;

        public FightService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request)
        {
            throw new NotImplementedException();
        }
    }
}